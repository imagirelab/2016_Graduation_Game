// 悪魔単体の処理をするクラス

using UnityEngine;
using StaticClass;

public class Demons : MonoBehaviour
{
    //悪魔のステータス
    [SerializeField, TooltipAttribute("悪魔のステータス")]
    public DemonsData status;

    //成長値
    bool changeGrowPoint = false;   //外部から変更があったかどうかのフラグ
    [SerializeField, TooltipAttribute("悪魔の成長値ポイント")]
    private DemonsGrowPointData growPoint;
    public DemonsGrowPointData GrowPoint
    {
        get { return growPoint; }

        set
        {
            growPoint = value;
            changeGrowPoint = true;
        }
    }

    //悪魔がなる魂オブジェクト
    public GameObject demonSpirit;

    //悪魔に渡される指示を出すクラス
    private DemonsOrder order;
    public DemonsOrder Order { set { order = value; } }

    //悪魔に作られた画面のプレイヤーのIDを入れておく変数
    private int playerID;
    public int PlayerID { set { playerID = value; } }
    
    private float time;                 //時間

    // 接触しているゲームオブジェクト
    private GameObject hitCollisionObject;
    
    void Start ()
    {
        //初期ステータス(プロパティからの設定情報)
        status.SetStutas();
        //外部からの変更がなかった場合初期の成長値を設定する
        if (!changeGrowPoint)
            growPoint.SetGrowPoint();

        //成長値によって今のステータスを算出する
        for (int i = 0; i < growPoint.CurrentHP_GrowPoint - growPoint.GetHP_GrowPoint; i++)
            status.CurrentHP += (int)(status.GetHP * 0.5f);
        for (int i = 0; i < growPoint.CurrentATK_GrowPoint - growPoint.GetATK_GrowPoint; i++)
            status.CurrentATK += (int)(status.GetATK * 0.5f);
        for (int i = 0; i < growPoint.CurrentSPEED_GrowPoint - growPoint.CurrentSPEED_GrowPoint; i++)
            status.CurrentSPEED += (int)(status.GetSPEED * 0.15f);
        //for (int i = 0; i < growPoint.CurrentAtackTime_GrowPoint - growPoint.CurrentAtackTime_GrowPoint; i++)
        //    status.CurrentAtackTime += (int)(status.GetAtackTime * 0.15f);
    }
	
	void Update ()
    {
        //オーダークラスがちゃんと設定されていれば処理する
        if(order)
            switch (order.CurrentOrder)
            {
                case DemonsOrder.Order.Castle:
                    break;
                case DemonsOrder.Order.Enemy:
                    EnemyOrder();
                    break;
                case DemonsOrder.Order.Building:
                    break;
                case DemonsOrder.Order.Spirit:
                    OrderSpirit();
                    break;
            }

        //死亡処理
        if (status.CurrentHP <= 0)
            Dead();
    }

    // 移動命令の処理
    public void MoveOrder(Vector3 targetPosition)
    {
        if (this.gameObject)
        {
            //目的地と0.1mより離れている場合の処理
            if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                //角度計算
                Vector3 moveDirection = (targetPosition - transform.position).normalized;
                //目的地への方向を見る
                transform.LookAt(transform.position + new Vector3(targetPosition.x, 0, targetPosition.z));
                //移動方向へ速度をSPEED分の与える
                this.GetComponent<Rigidbody>().velocity = moveDirection * status.CurrentSPEED;
            }
            //目的地に0.1mより近い距離になった場合の処理
            else
            {
                //速度を０に
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;

                //目的地への方向を見る
                transform.LookAt(transform.position + new Vector3(targetPosition.x, 0, targetPosition.z));
            }
        }
    }

    // 建造物に向かい攻撃する命令の処理
    public void BuildingOrder(GameObject target)
    {
        //ターゲットが何も無ければ処理しない
        if (target == null)
        {
            //速度を０に
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            return;
        }

        //目的地が攻撃距離より離れている場合の処理
        if (Vector3.Distance(this.transform.position, target.transform.position) > status.CurrentAtackLength + 6.0f)    //建物の外側の長さがわからないのでマジックナンバー使用
        {
            //角度計算
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;
            //目的地への方向を見る
            transform.LookAt(transform.position + new Vector3(target.transform.position.x, 0, target.transform.position.z));
            //移動方向へ速度をSPEED分の与える
            this.GetComponent<Rigidbody>().velocity = moveDirection * status.CurrentSPEED;
        }
        //目的地が攻撃距離内に入った場合の処理
        else
        {
            //速度を０に
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;

            //目的地への方向を見る
            transform.LookAt(transform.position + new Vector3(target.transform.position.x, 0, target.transform.position.z));

            //アタックタイムを満たしたら
            if (time > status.CurrentAtackTime)
            {
                time = 0;

                // お城クラスを持っていたら処理
                if(target.GetComponent<Castle>() != null)
                    target.GetComponent<Castle>().HPpro -= status.CurrentATK;

                //家クラスを持っていたら処理
                if (target.GetComponent<House>() != null)
                {
                    target.GetComponent<House>().HPpro -= status.CurrentATK;

                    //小屋が壊れたらコストを獲得
                    if (target.GetComponent<House>().HPpro <= 0)
                        transform.parent.GetComponent<PlayerCost>().AddCost(transform.parent.GetComponent<PlayerCost>().GetHouseCost);
                }
            }

            //1フレームあたりの時間を取得
            time += Time.deltaTime;

        }
    }
    
    //待機命令の処理(いらないかもしれないが)
    public void WaitOrder()
    {
        //今はとりあえず動きを止める
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }


    // 敵に攻撃する命令の処理
    public void EnemyOrder()
    {
        GameObject target = SolgierDataBase.getInstance().GetNearestObject(this.transform.position);

        //ターゲットがいない場合
        if (target == null)
        {
            //速度を０に
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            return;
        }

        Vector3 targetPosition = target.transform.position;

        //目的地への方向を見る
        transform.LookAt(new Vector3(targetPosition.x, 0, targetPosition.z));

        //目的地と0.1mより離れている場合の処理
        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            //角度計算
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            //移動方向へ速度をSPEED分の与える
            this.GetComponent<Rigidbody>().velocity = moveDirection * status.CurrentSPEED;
        }
        //目的地に0.1mより近い距離になった場合の処理
        else
        {
            //速度を０に
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    //魂の回収指示
    public void OrderSpirit()
    {
        GameObject target = SpiritDataBase.getInstance().GetNearestObject(this.transform.position);
        
        //ターゲットがいない場合
        if (target == null)
        {
            //速度を０に
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            return;
        }

        Vector3 targetPosition = target.transform.position;
        
        //目的地への方向を見る
        transform.LookAt(new Vector3(targetPosition.x, 0, targetPosition.z));

        //目的地と0.1mより離れている場合の処理
        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            //角度計算
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            //移動方向へ速度をSPEED分の与える
            this.GetComponent<Rigidbody>().velocity = moveDirection * status.CurrentSPEED;
        }
        //目的地に0.1mより近い距離になった場合の処理
        else
        {
            //速度を０に
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    //オブジェクトが衝突したときの処理
    void OnCollisionEnter(Collision collision)
    {
        ////接触したゲームオブジェクトを保存
        //hitCollisionObject = collision.gameObject;

        if (order)
            switch (order.CurrentOrder)
            {
                case DemonsOrder.Order.Castle:
                    break;
                case DemonsOrder.Order.Enemy:
                    break;
                case DemonsOrder.Order.Building:
                    break;
                case DemonsOrder.Order.Spirit:
                    break;
            }
    }

    //オブジェクトが触れている間
    void OnCollisionStay(Collision collision)
    {
        ////接触しているゲームオブジェクトを保存
        //hitCollisionObject = collision.gameObject;

        if (order)
            switch (order.CurrentOrder)
            {
                case DemonsOrder.Order.Castle:
                    break;
                case DemonsOrder.Order.Enemy:
                    Transform rootTransform = collision.gameObject.transform.root;
                    if (rootTransform.gameObject.tag == "Enemy")
                    {
                        time += Time.deltaTime;

                        if(time > status.CurrentAtackTime)
                        {
                            time = 0.0f;

                            rootTransform.gameObject.GetComponent<Soldier>().HPpro -= status.CurrentATK;

                            //兵士が死んだらコストをもらえる
                            if (rootTransform.GetComponent<Soldier>().HPpro <= 0)
                                transform.parent.GetComponent<PlayerCost>().AddCost(transform.parent.GetComponent<PlayerCost>().GetHouseCost);
                        }
                    }
                    break;
                case DemonsOrder.Order.Building:
                    break;
                case DemonsOrder.Order.Spirit:
                    break;
            }
    }

    //オブジェクトが離れた時
    void OnCollisionExit(Collision collision)
    {
        //離れたら戻す
        hitCollisionObject = null;
    }

    //トリガーに触れた時の瞬間の処理
    void OnTriggerEnter(Collider collider)
    {
        if (order)
            switch (order.CurrentOrder)
            {
                case DemonsOrder.Order.Castle:
                    break;
                case DemonsOrder.Order.Enemy:
                    break;
                case DemonsOrder.Order.Building:
                    break;
                case DemonsOrder.Order.Spirit:
                    Transform rootTransform = collider.gameObject.transform.root;
                    
                    //触れたトリガーが魂だったら
                    if (rootTransform.gameObject.tag == "Spirit")
                    {
                        this.gameObject.transform.root.gameObject.GetComponent<Player>().AddSpiritList(rootTransform.gameObject.GetComponent<DemonsSpirits>().GrowPoint);

                        if (rootTransform.IsChildOf(rootTransform))
                            foreach (Transform child in rootTransform)
                                Destroy(child.gameObject);
                        Destroy(rootTransform.gameObject);
                    }
                    break;
            }
    }

    //死んだときの処理
    void Dead()
    {
        //スピリットの生成
        GameObject spirit = (GameObject)Instantiate(demonSpirit, this.gameObject.transform.position, this.gameObject.transform.rotation);
        spirit.GetComponent<DemonsSpirits>().GrowPoint = growPoint;
        Destroy(gameObject);
    }
}
