using UnityEngine;
using System.Collections;

public class UnitAttack : MonoBehaviour
{
    [SerializeField]
    Unit.Type adType = Unit.Type.White;
    [SerializeField]
    Unit.Type unadType = Unit.Type.White;

    Coroutine cor;

    IEnumerator Attack()
    {
        Unit unit = gameObject.GetComponent<Unit>();

        while (true)
        {
            if (unit.IsAttack)
            {
                //攻撃対象がいることを確認してから攻撃
                if (unit.targetObject != null)
                {
                    gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                    //悪魔と兵士について
                    if (unit.targetObject.GetComponent<Unit>())
                    {
                        //倍率
                        float mag = 1.0f;

                        //倍率計算
                        if (unit.targetObject.GetComponent<Unit>().type == adType)
                            mag = 1.5f;
                        if (unit.targetObject.GetComponent<Unit>().type == unadType)
                            mag = 0.5f;

                        unit.targetObject.GetComponent<Unit>().status.CurrentHP -= Mathf.RoundToInt(unit.status.CurrentATK * mag);  //四捨五入

                        //親にプレイヤーコストを持っている(プレイヤー)場合のコスト処理
                        if (unit.targetObject.GetComponent<Unit>().status.CurrentHP <= 0)
                        {
                            if (unit.transform.root.gameObject.GetComponent<PlayerCost>())
                            {
                                PlayerCost playerCost = unit.transform.root.gameObject.GetComponent<PlayerCost>();
                                Player player = unit.transform.root.gameObject.GetComponent<Player>();

                                //parent.transform.root は　悪魔のRootつまりプレイヤー
                                player.AddCostList(playerCost.GetSoldierCost);
                            }
                        }
                    }
                    //建物への攻撃はこっち
                    if (unit.targetObject.GetComponent<House>())
                    {
                        unit.targetObject.GetComponent<House>().HPpro -= unit.status.CurrentATK;

                        //親にプレイヤーコストを持っている(プレイヤー)場合のコスト処理
                        if (unit.targetObject.GetComponent<House>().HPpro <= 0)
                        {
                            if (unit.transform.root.gameObject.GetComponent<Player>() != null)
                            {
                                Player player = unit.transform.root.gameObject.GetComponent<Player>();

                                //スポナーがあるときPlayerIDを登録
                                if (unit.targetObject.GetComponent<Spawner>() != null)
                                {
                                    //倒した奴のタグにする
                                    unit.targetObject.tag = player.transform.gameObject.tag;
                                    //子供も一緒に
                                    foreach (Transform child in unit.targetObject.transform)
                                        child.gameObject.tag = player.transform.gameObject.tag;

                                    unit.targetObject.GetComponent<Spawner>().CurrentPlayerID = player.playerID;
                                    unit.targetObject.GetComponent<Spawner>().CurrentTargetID = player.targetID;
                                }

                                if (unit.transform.root.gameObject.GetComponent<PlayerCost>())
                                {
                                    PlayerCost playerCost = unit.transform.root.gameObject.GetComponent<PlayerCost>();

                                    player.AddCostList(playerCost.GetSoldierCost);
                                }
                            }
                        }
                    }
                    //城への攻撃はこっち
                    if (unit.targetObject.GetComponent<DefenseBase>())
                    {
                        unit.targetObject.GetComponent<DefenseBase>().HPpro -= unit.status.CurrentATK;
                        unit.status.CurrentHP = 0;
                    }
                }
            }

            yield return new WaitForSeconds(unit.status.CurrentAtackTime);  //攻撃間隔
        }
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