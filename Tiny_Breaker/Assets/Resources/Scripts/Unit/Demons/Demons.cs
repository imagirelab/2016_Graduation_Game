﻿// 悪魔単体の処理をするクラス

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
    public DemonsOrder Order { set { order = value; } }

    //悪魔に作られた画面のプレイヤーのIDを入れておく変数
    private int playerID;
    public int PlayerID { set { playerID = value; } }
    
    // 触れているものの情報
    // 接触しているゲームオブジェクト
    private GameObject hitObject;

    GameObject castle;

    void Start ()
    {
        // 作られたときにリストに追加する
        DemonDataBase.getInstance().AddList(this.gameObject);

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

        IsDaed = false;

        castle = GameObject.Find("Castle");
    }

    //破壊されたときにリストから外す
    void OnDisable()
    {
        DemonDataBase.getInstance().RemoveList(this.gameObject);
    }

    void Update ()
    {
        //オーダークラスがちゃんと設定されていれば処理する
        if (order)
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
        if (status.CurrentHP <= 0)
            Dead();
    }


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

    //オブジェクトが衝突したときの処理
    void OnCollisionEnter(Collision collision)
    {
        //接触したゲームオブジェクトを保存
        hitObject = collision.gameObject;
    }

    //オブジェクトが触れている間
    void OnCollisionStay(Collision collision)
    {
        //接触しているゲームオブジェクトを保存
        hitObject = collision.gameObject;
    }

    //オブジェクトが離れた時
    void OnCollisionExit(Collision collision)
    {
        //離れたら戻す
        hitObject = null;
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
}