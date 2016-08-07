// 悪魔単体の処理をするクラス

using UnityEngine;
using StaticClass;

public class Demons : Unit
{
    //成長値
    bool changeGrowPoint = false;   //外部から変更があったかどうかのフラグ
    [SerializeField, TooltipAttribute("悪魔の成長値ポイント")]
    private GrowPoint growPoint;
    public GrowPoint GrowPoint
    {
        get { return growPoint; }
        set
        {
            growPoint = value;
            changeGrowPoint = true;
        }
    }

    //悪魔がなる魂オブジェクト
    public GameObject spirit;

    //悪魔に渡される指示を出すクラス
    private DemonsOrder order;
    public DemonsOrder Order { /*get { return order; }*/set { order = value; } }
    
    //お城
    GameObject castle;
    
    void Start()
    {
        // 作られたときにリストに追加する
        DemonDataBase.getInstance().AddList(this.gameObject);

        //死亡フラグ
        IsDaed = false;
        
        //お城を見つける
        castle = GameObject.Find("Castle");

        //成長値の初期設定
        if (!changeGrowPoint)
            growPoint.SetGrowPoint();

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
        //オーダークラスがちゃんと設定されていれば処理する
        if (order != null)
            switch (order.CurrentOrder)
            {
                case DemonsOrder.Order.Castle:
                    CastleOrder();
                    break;
                case DemonsOrder.Order.Enemy:
                    EnemyOrder();
                    break;
                case DemonsOrder.Order.Building:
                    BuildingOrder();
                    break;
                case DemonsOrder.Order.Spirit:
                    OrderSpirit();
                    break;
            }

        //死亡処理
        if (status.CurrentHP <= 0/* && loadEndFlag*/)
            Dead();
    }
    
    //お城への攻撃
    void CastleOrder()
    {
        //攻撃対象の設定
        targetObject = castle;

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
    
    //死んだときの処理
    void Dead()
    {
        IsDaed = true;

        //スピリットの生成
        GameObject instacespirit = (GameObject)Instantiate(spirit, this.gameObject.transform.position, this.gameObject.transform.rotation);
        instacespirit.GetComponent<Spirits>().GrowPoint = growPoint;
        Destroy(gameObject);
    }

    //ステータスの設定
    void SetStatus()
    {
        //基礎ステータスの代入
        status.SetStatus();

        //今のステータスを算出する
        for (int i = 0; i < growPoint.CurrentHP_GrowPoint - growPoint.GetHP_GrowPoint; i++)
            status.CurrentHP += (int)(status.GetHP * 0.5f);
        for (int i = 0; i < growPoint.CurrentATK_GrowPoint - growPoint.GetATK_GrowPoint; i++)
            status.CurrentATK += (int)(status.GetATK * 0.5f);
        for (int i = 0; i < growPoint.CurrentSPEED_GrowPoint - growPoint.CurrentSPEED_GrowPoint; i++)
            status.CurrentSPEED += status.GetSPEED * 0.15f;

        for (int i = 0; i < growPoint.CurrentAtackTime_GrowPoint - growPoint.CurrentAtackTime_GrowPoint; i++)
            status.CurrentAtackTime -= status.GetAtackTime * 0.05f;
    }
}
