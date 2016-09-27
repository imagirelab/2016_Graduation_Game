using UnityEngine;
using System.Collections;

public class UnitAttack : MonoBehaviour
{
    [SerializeField]    //Unity側から見たいとき用
    //[HideInInspector]
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

    //攻撃範囲
    [SerializeField]
    float atkRange = 2.0f;
    public float AtkRange { set { atkRange = value; } }

    //攻撃間隔
    [SerializeField]
    float atkTime = 1.0f;
    public float AtkTime { set { atkTime = value; } }

    //攻撃実行対象
    public GameObject target = null;

    Unit unit;

    Coroutine cor;

    IEnumerator Attack()
    {
        unit = gameObject.GetComponent<Unit>();

        while (true)
        {
            if (!isAttack)
            {
                if (unit.targetObject != null)
                {
                    RaycastHit hit;
                    Vector3 vec = unit.targetObject.transform.position - transform.position;
                    Ray ray = new Ray(transform.position, vec);
                    int layerMask = ~(1 << transform.gameObject.layer);  //自身のレイヤー番号以外にヒットするようにしたビット演算
                    if (Physics.SphereCast(ray, 3.0f, out hit, atkRange + transform.localScale.x, layerMask))
                    {
                        if (hit.collider.gameObject == unit.targetObject)
                        {
                            isAttack = true;

                            //範囲内に攻撃対象が入ってきたときの最初の攻撃対象の設定
                            if (target == null)
                            {
                                target = unit.targetObject;
                            }
                        }
                        else
                            isAttack = false;
                    }
                }
                yield return null;
            }
            else
            {
                //対象物が同じタグだったら仲間だから攻撃しない
                if (target != null)
                {
                    if (target.tag == transform.gameObject.tag)
                    {
                        //攻撃実行対象を戻す
                        target = null;
                    }
                }

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

                //対象の体力がなくなったらフラグを一旦戻す
                if (target == null)
                    isAttack = false;

                yield return new WaitForSeconds(atkTime);
            }
        }
    }

    void AttackUnit()
    {
        //悪魔と兵士について
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
            if (unit.transform.root.gameObject.GetComponent<PlayerCost>())
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

    void AttackHouse()
    {
        //建物への攻撃はこっち
        target.GetComponent<House>().HPpro -= unit.status.CurrentATK;

        //親にプレイヤーコストを持っている(プレイヤー)場合のコスト処理
        if (target.GetComponent<House>().HPpro <= 0)
        {
            if (unit.transform.root.gameObject.GetComponent<Player>() != null)
            {
                Player player = unit.transform.root.gameObject.GetComponent<Player>();

                //スポナーがあるときPlayerIDを登録
                if (target.GetComponent<Spawner>() != null)
                {
                    //倒した奴のタグにする
                    target.tag = player.transform.gameObject.tag;
                    //子供も一緒に
                    foreach (Transform child in target.transform)
                        child.gameObject.tag = player.transform.gameObject.tag;

                    target.GetComponent<Spawner>().CurrentPlayerID = player.playerID;
                    target.GetComponent<Spawner>().CurrentTargetID = player.targetID;
                }

                if (unit.transform.root.gameObject.GetComponent<PlayerCost>())
                {
                    PlayerCost playerCost = unit.transform.root.gameObject.GetComponent<PlayerCost>();

                    player.AddCostList(playerCost.GetSoldierCost);
                }
            }

            //攻撃実行対象を戻す
            target = null;
        }
    }

    void AttackDefenseBase()
    {
        //城への攻撃はこっち
        if (target.GetComponent<DefenseBase>())
            target.GetComponent<DefenseBase>().HPpro -= unit.status.CurrentATK;
    }

    void OnEnable()
    {
        cor = StartCoroutine(Attack());
    }

    void OnDisable()
    {
        StopCoroutine(cor);
    }
}