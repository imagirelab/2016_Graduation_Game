//アクティブの間攻撃を繰り返す

using UnityEngine;
using System.Collections;
using StaticClass;
using System.Collections.Generic;

public class UnitAttack : MonoBehaviour
{
    [SerializeField]
    Enum.Color_Type adType = Enum.Color_Type.White;
    [SerializeField]
    float admag = 1.5f;
    [SerializeField]
    Enum.Color_Type unadType = Enum.Color_Type.White;
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

    //攻撃実行対象
    GameObject target = null;
    public GameObject Target { get { return target; } }

    [SerializeField, TooltipAttribute("範囲攻撃するときの範囲")]
    float roundRenge = 1.5f;

    Unit unit;
    Coroutine cor;

    IEnumerator Attack()
    {
        unit = gameObject.GetComponent<Unit>();

        yield return new WaitForSeconds(setDelayTime);

        while (true)
        {
            yield return new WaitForSeconds(atkTime * atkDamageDelayRate);

            //攻撃中心対象がいなくなったら再登録
            if (target == null)
                target = unit.targetObject;

            //対象物が同じタグだったら仲間だから攻撃しない
            if (target != null)
                if (target.tag == transform.gameObject.tag)
                    target = null;

            List<GameObject> targetList = new List<GameObject>();
            //まずは中心攻撃対象だけを登録
            targetList.Add(target);
            //範囲攻撃
            if (unit.RoundAttack)
            {
                //範囲攻撃に使うスフィア
                int layerMask = ~(1 << transform.gameObject.layer | 1 << 18 | 1 << 21);  //レイキャストが省くレイヤーのビット演算
                Collider[] allhit = Physics.OverlapSphere(target.transform.position, roundRenge, layerMask);
                //中心攻撃対象の周りを登録
                foreach (Collider e in allhit)
                    if(e.gameObject != target)
                        targetList.Add(e.gameObject);
            }

            //種類によって攻撃処理
            foreach (GameObject e in targetList)
            {
                //攻撃対象がいることを確認してから攻撃
                if (e != null)
                {
                    transform.LookAt(e.transform.position);

                    if (e.GetComponent<Unit>())
                        AttackUnit(e);
                    else if (e.GetComponent<House>())
                        AttackHouse(e);
                    else if (e.GetComponent<DefenseBase>())
                        AttackDefenseBase(e);
                }
            }

            yield return null;
            yield return new WaitForSeconds(atkTime - (atkTime * atkDamageDelayRate));
        }
    }

    //悪魔と兵士について
    void AttackUnit(GameObject _target)
    {
        Unit unitComp = _target.GetComponent<Unit>();

        //倍率
        float mag = 1.0f;

        //倍率計算
        if (unitComp.type == adType)
            mag = admag;
        if (unitComp.type == unadType)
            mag = unadmag;

        unitComp.AnyDamage(Mathf.RoundToInt(unit.status.CurrentATK * mag), unit);

        //親にプレイヤーコストを持っている(プレイヤー)場合のコスト処理
        if (unitComp.status.CurrentHP <= 0)
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
    void AttackHouse(GameObject _target)
    {
        GameObject rootObject = unit.transform.root.gameObject;
        House houseComp = _target.GetComponent<House>();

        switch (rootObject.tag)
        {
            case "Player1":
                houseComp.HPpro += unit.status.CurrentATK;
                break;
            case "Player2":
                houseComp.HPpro -= unit.status.CurrentATK;
                break;
            default:
                break;
        }

        //親が小屋クラスを持っている(プレイヤー)場合のコスト処理
        if (houseComp.HPpro <= -houseComp.GetHP ||
            houseComp.HPpro >= houseComp.GetHP)
        {
            //死んだフラグを立てる
            houseComp.IsDead = true;
            //リストからはずす
            BuildingDataBase.getInstance().RemoveList(_target);

            if (rootObject.GetComponent<Player>() != null)
            {
                Player player = rootObject.GetComponent<Player>();

                //スポナーがあるときPlayerIDを登録
                if (_target.GetComponent<Spawner>() != null)
                {
                    //事前にタグを保存しておく
                    houseComp.OldTag = _target.tag;

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
    void AttackDefenseBase(GameObject _target)
    {
        if (_target.GetComponent<DefenseBase>())
            _target.GetComponent<DefenseBase>().HPpro -= unit.status.CurrentATK;
    }

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