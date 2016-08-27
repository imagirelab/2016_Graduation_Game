﻿// 悪魔単体の処理をするクラス

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

    //悪魔がなる魂オブジェクト
    public GameObject spirit;

    //悪魔に渡される指示を出すクラス
    private DemonsOrder order;
    public DemonsOrder Order { set { order = value; } }
    
    //お城
    GameObject castle;
    
    void Start()
    {
        // 作られたときにリストに追加する
        DemonDataBase.getInstance().AddList(this.gameObject);

        //死亡フラグ
        IsDead = false;
        
        //お城を見つける
        castle = GameObject.Find("Castle");
        
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
        //EnemyOrder();
        //CastleOrder();
        //OrderSpirit();

        //終わり
        if (transform.parent.gameObject.GetComponent<Player>().target == null)
        {
            OrderWait();
            return;
        }

        //攻撃対象の設定
        if (transform.parent.gameObject != null)
        {
            targetObject = transform.parent.gameObject.GetComponent<Player>().target;
            if (SolgierDataBase.getInstance().GetNearestObject(this.transform.position) != null)
                if (Vector3.Distance(this.transform.position, SolgierDataBase.getInstance().GetNearestObject(this.transform.position).transform.position) <
                        Vector3.Distance(this.transform.position, targetObject.transform.position))
                    targetObject = SolgierDataBase.getInstance().GetNearestObject(this.transform.position);
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
        if(transform.parent.gameObject != null)
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
            status.CurrentSPEED += status.GetSPEED * 2.0f/*0.15f*/;

        for (int i = 0; i < growPoint.CurrentAtackTime_GrowPoint - growPoint.CurrentAtackTime_GrowPoint; i++)
            status.CurrentAtackTime -= status.GetAtackTime * 0.05f;

        status.MaxHP = status.CurrentHP;

        loiteringSPEED = status.CurrentSPEED;
    }
}
