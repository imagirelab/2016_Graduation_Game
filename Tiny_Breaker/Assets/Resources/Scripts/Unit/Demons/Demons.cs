// 悪魔単体の処理をするクラス

using UnityEngine;
using StaticClass;

public class Demons : Unit
{
    //成長値
    [SerializeField, TooltipAttribute("悪魔の成長値ポイント")]
    private GrowPoint growPoint;
    public GrowPoint GrowPoint
    {
        get { return growPoint; }
        set { growPoint = value; }
    }

    ////悪魔がなる魂オブジェクト
    //[SerializeField]
    //GameObject spirit;

    ////悪魔に渡される指示を出すクラス
    //private DemonsOrder order;
    //public DemonsOrder Order { set { order = value; } }

    void Start()
    {
        // 作られたときにリストに追加する
        DemonDataBase.getInstance().AddList(this.gameObject, transform.gameObject.tag);

        //死亡フラグ
        IsDead = false;

        //巡回ルート
        if (gameObject.transform.parent == null)
            loiteringPointObj = new Transform[] { transform };

        //ステータスの決定
        SetStatus();
    }

    //破壊されたときにリストから外す
    void OnDisable()
    {
        DemonDataBase.getInstance().RemoveList(this.gameObject);
    }

    void Update()
    {
        //終わり
        if (transform.parent.gameObject.GetComponent<Player>().target == null)
        {
            OrderWait();
            return;
        }

        //攻撃対象の設定
        if (transform.parent != null)
        {
            //goalObject = transform.parent.gameObject.GetComponent<Player>().target;

            //プレイヤーのTarget
            targetObject = goalObject;

            //悪魔
            GameObject nearestObject = DemonDataBase.getInstance().GetNearestObject(targetTag, this.transform.position);
            if (nearestObject != null)
                if (Vector3.Distance(this.transform.position, nearestObject.transform.position) <
                        Vector3.Distance(this.transform.position, targetObject.transform.position))
                    targetObject = nearestObject;

            //兵士
            GameObject nearSol = SolgierDataBase.getInstance().GetNearestObject(this.transform.position);
            if (nearSol != null)
                if (nearSol.tag != transform.gameObject.tag)
                    if (Vector3.Distance(this.transform.position, nearSol.transform.position) <
                            Vector3.Distance(this.transform.position, targetObject.transform.position))
                        targetObject = nearSol;

            //建物
            GameObject nearBuild = BuildingDataBase.getInstance().GetNearestObject(this.transform.position);
            if (nearBuild != null)
                if (nearBuild.tag != transform.gameObject.tag)
                    if (Vector3.Distance(this.transform.position, nearBuild.transform.position) <
                            Vector3.Distance(this.transform.position, targetObject.transform.position))
                        targetObject = nearBuild;
        }

        if (!IsFind)
            Loitering(loiteringPointObj);
        else
            Move(targetObject);

        ////オーダークラスがちゃんと設定されていれば処理する
        //if (order != null)
        //    switch (order.CurrentOrder)
        //    {
        //        case DemonsOrder.Order.Castle:
        //            CastleOrder();
        //            break;
        //        case DemonsOrder.Order.Enemy:
        //            EnemyOrder();
        //            break;
        //        case DemonsOrder.Order.Building:
        //            BuildingOrder();
        //            break;
        //        case DemonsOrder.Order.Spirit:
        //            OrderSpirit();
        //            break;
        //        default:    //待機
        //            OrderWait();
        //            break;
        //    }

        //死亡処理
        if (status.CurrentHP <= 0)
            Dead();
    }

    //お城への攻撃
    void CastleOrder()
    {
        //攻撃対象の設定
        //targetObject = castle;

        //移動
        Move(targetObject);
    }

    // 敵に攻撃する命令の処理
    void EnemyOrder()
    {
        //攻撃対象の設定
        targetObject = SolgierDataBase.getInstance().GetNearestObject(this.transform.position);

        //移動
        Move(targetObject);
    }

    // 建造物に向かい攻撃する命令の処理
    void BuildingOrder()
    {
        //攻撃対象の設定
        targetObject = BuildingDataBase.getInstance().GetNearestObject(this.transform.position);

        //移動
        Move(targetObject);
    }

    //魂の回収指示
    void OrderSpirit()
    {
        targetObject = SpiritDataBase.getInstance().GetNearestObject(this.transform.position);

        //移動
        Move(targetObject);
    }

    //待機指示
    void OrderWait()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //NavMeshAgentを止める
        if (GetComponent<NavMeshAgent>())
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = this.transform.position;
        }
    }

    //死んだときの処理
    void Dead()
    {
        IsDead = true;

        //死んだ直後に魂を回収してみる
        if (transform.parent.gameObject != null)
            transform.parent.gameObject.GetComponent<Player>().AddSpiritList(growPoint);

        ////スピリットの生成
        //GameObject instacespirit = (GameObject)Instantiate(spirit, this.gameObject.transform.position, this.gameObject.transform.rotation);
        //instacespirit.GetComponent<Spirits>().GrowPoint = growPoint;

        Destroy(gameObject);
    }

    //ステータスの設定
    public void SetStatus()
    {
        //基礎ステータスの代入
        status.SetStatus();

        //今のステータスを算出する
        for (int i = 0; i < growPoint.CurrentHP_GrowPoint - growPoint.GetHP_GrowPoint; i++)
            status.CurrentHP += (int)(status.GetHP * 0.5f);
        for (int i = 0; i < growPoint.CurrentATK_GrowPoint - growPoint.GetATK_GrowPoint; i++)
            status.CurrentATK += (int)(status.GetATK * 0.5f);
        for (int i = 0; i < growPoint.CurrentSPEED_GrowPoint - growPoint.GetSPEED_GrowPoint; i++)
            status.CurrentSPEED += status.GetSPEED * 0.15f;
        for (int i = 0; i < growPoint.CurrentAtackTime_GrowPoint - growPoint.GetAtackTime_GrowPoint; i++)
            status.CurrentAtackTime -= status.GetAtackTime * 0.05f;

        //カンスト
        if (status.CurrentHP >= 9999)
            status.CurrentHP = 9999;
        if (status.CurrentATK >= 2000)
            status.CurrentATK = 2000;
        if (status.CurrentSPEED >= 10)
            status.CurrentSPEED = 10;
        if (status.CurrentAtackTime <= 0.5f)    //１フレーム以下にならない方がいいかも
            status.CurrentAtackTime = 0.5f;

        status.MaxHP = status.CurrentHP;

        loiteringSPEED = status.CurrentSPEED;
    }
}
