//攻撃範囲に関するクラス

using UnityEngine;
using NCMB;

public class AttackRange : UnitTrigger
{
    float time;                 //時間
    
    void Start()
    {
        //親にUnitClassを継承しているスクリプトを持っていたら登録する
        if (transform.parent.GetComponent<Unit>())
            parent = transform.parent.gameObject.GetComponent<Unit>();
        else
            Debug.Log("UnitTrigger: parent =" + parent);

        parent.IsAttack = false;
        
        time = 0.0f;
    }

    void Update ()
    {
        //アタックタイムを満たしていて攻撃フラグが立っていたら攻撃
        if (time > parent.status.CurrentAtackTime && parent.IsAttack)
        {
            time = 0;

            //攻撃対象がいることを確認してから攻撃
            if (hitTarget != null && parent.targetObject != null)
            {
                //悪魔と兵士について
                if (hitTarget.GetComponent<Unit>())
                {
                    hitTarget.GetComponent<Unit>().status.CurrentHP -= parent.status.CurrentATK;

                    //親にプレイヤーコストを持っている(プレイヤー)場合のコスト処理
                    if (hitTarget.GetComponent<Unit>().status.CurrentHP <= 0)
                    {
                        if (parent.transform.root.gameObject.GetComponent<PlayerCost>())
                        {
                            PlayerCost playerCost = parent.transform.root.gameObject.GetComponent<PlayerCost>();
                            //playerCost.AddCost(playerCost.GetSoldierCost);

                            NCMBObject spiritObj = new NCMBObject("CostData");

                            spiritObj["Cost"] = playerCost.GetSoldierCost.ToString();

                            spiritObj.SaveAsync();
                        }
                    }
                }
                //建物への攻撃はこっち
                if (hitTarget.GetComponent<House>())
                {
                    hitTarget.GetComponent<House>().HPpro -= parent.status.CurrentATK;

                    //親にプレイヤーコストを持っている(プレイヤー)場合のコスト処理
                    if (hitTarget.GetComponent<House>().HPpro <= 0)
                    {
                        if (parent.transform.root.gameObject.GetComponent<PlayerCost>())
                        {
                            PlayerCost playerCost = parent.transform.root.gameObject.GetComponent<PlayerCost>();
                            //playerCost.AddCost(playerCost.GetHouseCost);
                            
                            NCMBObject spiritObj = new NCMBObject("CostData");

                            spiritObj["Cost"] = playerCost.GetHouseCost.ToString();

                            spiritObj.SaveAsync();
                        }
                    }
                }
                //城への攻撃はこっち
                if (parent.targetObject.GetComponent<Castle>())
                {
                    parent.targetObject.GetComponent<Castle>().HPpro -= parent.status.CurrentATK;
                }
            }
            else
            {
                hitTarget = null;
                hitFlag = false;
            }
        }

        //攻撃フラグの更新
        parent.IsAttack = hitFlag;

        //1フレームあたりの時間を取得
        time += Time.deltaTime;
    }
}
