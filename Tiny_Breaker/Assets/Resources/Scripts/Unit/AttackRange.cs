//攻撃範囲に関するクラス

using UnityEngine;

public class AttackRange : UnitTrigger
{
    [SerializeField]
    Unit.Type adType = Unit.Type.White;
    [SerializeField]
    Unit.Type unadType = Unit.Type.White;

    float time;                 //時間
    
    void Start()
    {
        //親にUnitClassを継承しているスクリプトを持っていたら登録する
        if (transform.parent.GetComponent<Unit>())
            parent = transform.parent.gameObject.GetComponent<Unit>();
        else
        {
            parent = new Unit();
            Debug.Log("UnitTrigger: parent =" + parent);
        }

        parent.IsAttack = false;
        
        time = 0.0f;
    }

    void Update ()
    {
        //攻撃フラグの更新
        parent.IsAttack = hitFlag;
        
        if (parent.targetObject == null)
        {
            parent.IsAttack = false;
            hitFlag = false;
        }
        
        //1フレームあたりの時間を取得
        time -= Time.deltaTime;

        //アタックタイムを満たしていて攻撃フラグが立っていたら攻撃
        if (time < 0.0f && parent.IsAttack)
        {
            time = parent.status.CurrentAtackTime;

            //攻撃対象がいることを確認してから攻撃
            if (hitTarget != null && parent.targetObject != null)
            {
                parent.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                //悪魔と兵士について
                if (parent.targetObject.GetComponent<Unit>())
                {
                    //倍率
                    float mag = 1.0f;

                    //倍率計算
                    if (parent.targetObject.GetComponent<Unit>().type == adType)
                        mag = 1.5f;
                    if (parent.targetObject.GetComponent<Unit>().type == unadType)
                        mag = 0.5f;

                    parent.targetObject.GetComponent<Unit>().status.CurrentHP -= Mathf.RoundToInt(parent.status.CurrentATK * mag);  //四捨五入

                    //親にプレイヤーコストを持っている(プレイヤー)場合のコスト処理
                    if (parent.targetObject.GetComponent<Unit>().status.CurrentHP <= 0)
                    {
                        if (parent.transform.root.gameObject.GetComponent<PlayerCost>())
                        {
                            PlayerCost playerCost = parent.transform.root.gameObject.GetComponent<PlayerCost>();
                            Player player = parent.transform.root.gameObject.GetComponent<Player>();

                            //parent.transform.root は　悪魔のRootつまりプレイヤー
                            player.AddCostList(playerCost.GetSoldierCost);
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
                        if (parent.transform.root.gameObject.GetComponent<Player>() != null)
                        {
                            Player player = parent.transform.root.gameObject.GetComponent<Player>();

                            //スポナーがあるときPlayerIDを登録
                            if (parent.targetObject.GetComponent<Spawner>() != null)
                            {
                                //倒した奴のタグにする
                                parent.targetObject.tag = player.transform.gameObject.tag;
                                //子供も一緒に
                                foreach(Transform child in parent.targetObject.transform)
                                    child.gameObject.tag = player.transform.gameObject.tag;
                                
                                parent.targetObject.GetComponent<Spawner>().CurrentPlayerID = player.playerID;
                                parent.targetObject.GetComponent<Spawner>().CurrentTargetID = player.targetID;
                            }

                            if (parent.transform.root.gameObject.GetComponent<PlayerCost>())
                            {
                                PlayerCost playerCost = parent.transform.root.gameObject.GetComponent<PlayerCost>();

                                player.AddCostList(playerCost.GetSoldierCost);
                            }
                        }
                    }
                }
                //城への攻撃はこっち
                if (parent.targetObject.GetComponent<DefenseBase>())
                {
                    parent.targetObject.GetComponent<DefenseBase>().HPpro -= parent.status.CurrentATK;
                    parent.status.CurrentHP = 0;
                }
            }
            else
            {
                hitTarget = null;
                hitFlag = false;
            }
        }
        
        //Debug.Log(hitTarget);
        //Debug.Log(parent.targetObject);
    }
}
