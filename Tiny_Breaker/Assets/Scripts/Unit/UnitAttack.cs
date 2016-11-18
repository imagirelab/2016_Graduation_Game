//アクティブの間攻撃を繰り返す

using UnityEngine;
using System.Collections;
using StaticClass;
using System.Collections.Generic;

public class UnitAttack : MonoBehaviour
{
    //[SerializeField]    //Unity側から見たいとき用
    [HideInInspector]
    bool isAttack = false;
    public bool IsAttack { get { return isAttack; } }

    [SerializeField]
    Unit.Type adType = Unit.Type.White;
    [SerializeField]
    float admag = 1.5f;
    [SerializeField]
    Unit.Type unadType = Unit.Type.White;
    [SerializeField]
    float unadmag = 0.5f;
    
    //攻撃間隔
    [SerializeField]
    float atkTime = 1.0f;
    public float AtkTime { set { atkTime = value; } }

    //攻撃を実際に計算するときの攻撃時間全体を見てどのあたりで計算を行うかの割合
    //攻撃間隔　２秒　atkDamageDelayRate　0.4fの場合
    //0.8秒の時に計算を行う
    [SerializeField, Range(0, 1.0f), TooltipAttribute("ダメージ計算を行うタイミングの割合")]
    float atkDamageDelayRate = 0.0f;

    [SerializeField, TooltipAttribute("構え時間")]
    float setDelayTime = 0.0f;

    //攻撃実行対象(単体)
    [SerializeField]
    GameObject target = null;
    public GameObject Target { get { return target; } }

    //攻撃実行対象(複数)
    [SerializeField]
    List<GameObject> allTarget;
    public List<GameObject> AllTarget { get { return allTarget; } }

    Unit unit;

    Coroutine cor;

    IEnumerator Attack()
    {
        unit = gameObject.GetComponent<Unit>();

        yield return new WaitForSeconds(setDelayTime);

        while (true)
        {
            yield return new WaitForSeconds(atkTime * atkDamageDelayRate);

            #region 単体攻撃
            if (unit.state == Unit.State.Attack)
            {
                if (target == null)
                    target = unit.targetObject;

                //対象物が同じタグだったら仲間だから攻撃しない
                if (target != null)
                    if (target.tag == transform.gameObject.tag)
                        //攻撃実行対象を戻す
                        target = null;

                //攻撃対象がいることを確認してから攻撃
                if (target != null)
                {
                    transform.LookAt(target.transform.position);

                    if (target.GetComponent<Unit>())
                        AttackUnit();
                    else if (target.GetComponent<House>())
                        AttackHouse();
                    else if (target.GetComponent<DefenseBase>())
                        AttackDefenseBase();
                }
            }
            #endregion
            #region 複数攻撃
            else if (unit.state == Unit.State.ALLAttack)
            {
                allTarget.Clear();
                for(int i = 0; i < unit.allTargetObject.Count; i++)
                {
                    Debug.Log("unitCount :" + unit.allTargetObject.Count);
                    if (allTarget.Count != unit.allTargetObject.Count)
                    {
                        allTarget.Add(unit.allTargetObject[i]);
                    }
                    Debug.Log("targetCount : " + allTarget.Count);
                    //対象物が同じタグだったら仲間だから攻撃しない
                    if (allTarget[i] != null)
                        if (allTarget[i].tag == transform.gameObject.tag)
                            //攻撃実行対象を戻す
                            allTarget.RemoveAt(i);

                    //攻撃対象がいることを確認してから攻撃
                    if (allTarget[i] != null)
                    {
                        transform.LookAt(allTarget[i].transform.position);

                        if (allTarget[i].GetComponent<Unit>())
                            ALLAttackUnit(allTarget[i], i);
                        else if (allTarget[i].GetComponent<House>())
                            ALLAttackHouse(allTarget[i], i);
                        else if (allTarget[i].GetComponent<DefenseBase>())
                            ALLAttackDefenseBase(allTarget[i], i);
                    }
                }
            }
            #endregion

            yield return null;
            yield return new WaitForSeconds(atkTime - (atkTime * atkDamageDelayRate));
        }
    }

    #region 単体攻撃
    //悪魔と兵士について
    void AttackUnit()
    {
        //倍率
        float mag = 1.0f;

        //倍率計算
        if (target.GetComponent<Unit>().type == adType)
            mag = admag;
        if (target.GetComponent<Unit>().type == unadType)
            mag = unadmag;

        target.GetComponent<Unit>().status.CurrentHP -= Mathf.RoundToInt(unit.status.CurrentATK * mag);  //四捨五入

        //親にプレイヤーコストを持っている(プレイヤー)場合のコスト処理
        if (target.GetComponent<Unit>().status.CurrentHP <= 0)
        {
            if (target.GetComponent<Soldier>() && unit.transform.root.gameObject.GetComponent<PlayerCost>())
            {
                PlayerCost playerCost = unit.transform.root.gameObject.GetComponent<PlayerCost>();
                Player player = unit.transform.root.gameObject.GetComponent<Player>();

                //parent.transform.root は　悪魔のRootつまりプレイヤー
                player.AddCostList(playerCost.GetSoldierCost);
            }

            //攻撃実行対象を戻す
            target = null;
        }
    }

    //建物への攻撃はこっち
    void AttackHouse()
    {
        GameObject rootObject = unit.transform.root.gameObject;

        switch (rootObject.tag)
        {
            case "Player1":
                target.GetComponent<House>().HPpro += unit.status.CurrentATK;
                break;
            case "Player2":
                target.GetComponent<House>().HPpro -= unit.status.CurrentATK;
                break;
            default:
                break;
        }

        //親が小屋クラスを持っている(プレイヤー)場合のコスト処理
        if (target.GetComponent<House>().HPpro <= -target.GetComponent<House>().GetHP ||
            target.GetComponent<House>().HPpro >= target.GetComponent<House>().GetHP)
        {
            //死んだフラグを立てる
            target.GetComponent<House>().IsDead = true;
            //リストからはずす
            BuildingDataBase.getInstance().RemoveList(target);

            if (rootObject.GetComponent<Player>() != null)
            {
                Player player = rootObject.GetComponent<Player>();

                //スポナーがあるときPlayerIDを登録
                if (target.GetComponent<Spawner>() != null)
                {
                    //事前にタグを保存しておく
                    target.GetComponent<House>().OldTag = target.tag;

                    //倒した奴のタグにする
                    target.tag = player.transform.gameObject.tag;
                    //子供も一緒に
                    foreach (Transform child in target.transform)
                        child.gameObject.tag = player.transform.gameObject.tag;

                    target.GetComponent<Spawner>().CurrentPlayerID = player.playerID;
                    target.GetComponent<Spawner>().CurrentTargetID = player.targetID;
                }

                //コストの計算
                if (unit.transform.root.gameObject.GetComponent<PlayerCost>())
                {
                    PlayerCost playerCost = unit.transform.root.gameObject.GetComponent<PlayerCost>();

                    player.AddCostList(playerCost.GetHouseCost);
                }
            }

            //攻撃実行対象を戻す
            target = null;
        }
    }

    //城への攻撃はこっち
    void AttackDefenseBase()
    {
        if (target.GetComponent<DefenseBase>())
            target.GetComponent<DefenseBase>().HPpro -= unit.status.CurrentATK;
    }
    #endregion

    #region 複数攻撃
    //悪魔と兵士について
    void ALLAttackUnit(GameObject _target, int num)
    {
        //倍率
        float mag = 1.0f;

        //倍率計算
        if (_target.GetComponent<Unit>().type == adType)
            mag = admag;
        if (_target.GetComponent<Unit>().type == unadType)
            mag = unadmag;

        _target.GetComponent<Unit>().status.CurrentHP -= Mathf.RoundToInt(unit.status.CurrentATK * mag);  //四捨五入
        Debug.Log("call");

        //親にプレイヤーコストを持っている(プレイヤー)場合のコスト処理
        if (_target.GetComponent<Unit>().status.CurrentHP <= 0)
        {
            if (_target.GetComponent<Soldier>() && unit.transform.root.gameObject.GetComponent<PlayerCost>())
            {
                PlayerCost playerCost = unit.transform.root.gameObject.GetComponent<PlayerCost>();
                Player player = unit.transform.root.gameObject.GetComponent<Player>();

                //parent.transform.root は　悪魔のRootつまりプレイヤー
                player.AddCostList(playerCost.GetSoldierCost);
            }
        }
    }

    //建物への攻撃はこっち
    void ALLAttackHouse(GameObject _target, int num)
    {
        GameObject rootObject = unit.transform.root.gameObject;

        switch (rootObject.tag)
        {
            case "Player1":
                _target.GetComponent<House>().HPpro += unit.status.CurrentATK;
                break;
            case "Player2":
                _target.GetComponent<House>().HPpro -= unit.status.CurrentATK;
                break;
            default:
                break;
        }

        //親が小屋クラスを持っている(プレイヤー)場合のコスト処理
        if (_target.GetComponent<House>().HPpro <= -_target.GetComponent<House>().GetHP ||
            _target.GetComponent<House>().HPpro >= _target.GetComponent<House>().GetHP)
        {
            //死んだフラグを立てる
            _target.GetComponent<House>().IsDead = true;
            //リストからはずす
            BuildingDataBase.getInstance().RemoveList(_target);

            if (rootObject.GetComponent<Player>() != null)
            {
                Player player = rootObject.GetComponent<Player>();

                //スポナーがあるときPlayerIDを登録
                if (_target.GetComponent<Spawner>() != null)
                {
                    //事前にタグを保存しておく
                    _target.GetComponent<House>().OldTag = _target.tag;

                    //倒した奴のタグにする
                    _target.tag = player.transform.gameObject.tag;
                    //子供も一緒に
                    foreach (Transform child in _target.transform)
                        child.gameObject.tag = player.transform.gameObject.tag;

                    _target.GetComponent<Spawner>().CurrentPlayerID = player.playerID;
                    _target.GetComponent<Spawner>().CurrentTargetID = player.targetID;
                }

                //コストの計算
                if (unit.transform.root.gameObject.GetComponent<PlayerCost>())
                {
                    PlayerCost playerCost = unit.transform.root.gameObject.GetComponent<PlayerCost>();

                    player.AddCostList(playerCost.GetHouseCost);
                }
            }
        }
    }

    //城への攻撃はこっち
    void ALLAttackDefenseBase(GameObject _target, int num)
    {
        if (_target.GetComponent<DefenseBase>())
            _target.GetComponent<DefenseBase>().HPpro -= unit.status.CurrentATK;
    }
    #endregion

    void OnEnable()
    {
        cor = StartCoroutine(Attack());
    }

    void OnDisable()
    {
        //攻撃実行対象を戻す
        target = null;

        StopCoroutine(cor);
    }
}