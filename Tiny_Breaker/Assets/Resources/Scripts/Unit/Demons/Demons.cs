// 悪魔単体の処理をするクラス

using UnityEngine;
using StaticClass;
using System.Collections.Generic;
using NCMB;

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

    private bool loadEndFlag = false;
    private bool loadStartFlag = false;

    void Start()
    {
        // 作られたときにリストに追加する
        DemonDataBase.getInstance().AddList(this.gameObject);

        IsDaed = false;

        Debug.Log("DemonSummoned by Hp:" + status.CurrentHP);

        castle = GameObject.Find("Castle");

        Debug.Log(growPoint.CurrentSPEED_GrowPoint);
    }

    //破壊されたときにリストから外す
    void OnDisable()
    {
        DemonDataBase.getInstance().RemoveList(this.gameObject);
    }

    void Update()
    {
        if(!loadEndFlag)
        {
            if (!loadStartFlag)
            {
                loadStartFlag = true;
                Demon_SetStatus();
            }
            if(status.CurrentHP > 0)
                loadEndFlag = true;
        }

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
        if (status.CurrentHP <= 0 && loadEndFlag)
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

    void Demon_SetStatus()
    {
        //クエリを作成
        NCMBQuery<NCMBObject> demonStatus = new NCMBQuery<NCMBObject>("DemonData");

        //PlayerNoが1のものを取得
        demonStatus.WhereEqualTo("PlayerNo", "1");

        //検索
        demonStatus.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            //検索失敗時
            if (e != null)
            {
                Debug.Log(e.ToString());
                return;
            }
            else
            {
                foreach (NCMBObject ncmbObj in objList)
                {
                    //成長値によって今のステータスを算出する
                    status.CurrentHP = System.Convert.ToInt32(ncmbObj["HP"].ToString());
                    status.CurrentATK = System.Convert.ToInt32(ncmbObj["ATK"].ToString());
                    status.CurrentSPEED = System.Convert.ToInt32(ncmbObj["DEX"].ToString());
                    status.CurrentAtackTime -= status.GetAtackTime * 0.05f;
                    status.CurrentAtackTime = 0.1f;
                }
            }
        });
    }
}
