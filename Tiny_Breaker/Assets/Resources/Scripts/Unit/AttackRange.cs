//攻撃範囲に関するクラス

using UnityEngine;

public class AttackRange : UnitTrigger
{
    float time;                 //時間
    
    void Start()
    {
        //親にUnitClassを継承しているスクリプトを持っていたら登録する
        if (transform.parent.GetComponent<Unit>())
            parent = transform.parent.GetComponent<Unit>();
        else
            Debug.Log("UnitTrigger: parent =" + parent);

        parent.IsAttack = false;
        
        time = 0.0f;
    }

    void Update () {
        
        //アタックタイムを満たしていて攻撃フラグが立っていたら攻撃
        if (time > parent.status.CurrentAtackTime && parent.IsAttack)
        {
            time = 0;

            //攻撃対象がいることを確認してから攻撃
            if (hitTarget != null && parent.targetObject != null)
            {
                //悪魔と兵士について
                if (parent.targetObject.GetComponent<Unit>())
                {
                    parent.targetObject.GetComponent<Unit>().status.CurrentHP -= parent.status.CurrentATK;

                    //親にプレイヤーコストを持っている(プレイヤー)場合のコスト処理
                    if (parent.targetObject.GetComponent<Unit>().status.CurrentHP <= 0)
                    {
                        if (parent.transform.root.gameObject.GetComponent<PlayerCost>())
                        {
                            PlayerCost playerCost = parent.transform.root.gameObject.GetComponent<PlayerCost>();
                            playerCost.AddCost(playerCost.GetSoldierCost);
                        }
                    }
                }
                //建物への攻撃はこっち
                if (parent.targetObject.GetComponent<House>())
                {
                    parent.targetObject.GetComponent<House>().HPpro -= parent.status.CurrentATK;

                    //親にプレイヤーコストを持っている(プレイヤー)場合のコスト処理
                    if (parent.targetObject.GetComponent<House>().HPpro <= 0)
                    {
                        if (parent.transform.root.gameObject.GetComponent<PlayerCost>())
                        {
                            PlayerCost playerCost = parent.transform.root.gameObject.GetComponent<PlayerCost>();
                            playerCost.AddCost(playerCost.GetHouseCost);
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
                parent.IsAttack = false;
        }

        //1フレームあたりの時間を取得
        time += Time.deltaTime;
    }
}
