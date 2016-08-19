using UnityEngine;
using System.Collections.Generic;
using NCMB;

//大きい画面でのプレイヤークラス(スマホの情報の受け渡しとかプレイヤー番号とか送受信の解析とか)
public class Player : MonoBehaviour
{
    #region フィールド

    //プレイヤーを識別する番号
    //誰の悪魔が魂の回収を行ったのかを判断するため必要だと思った
    public int playerID = 1;

    //スマホ側に送信するのに一時的にデータをためておく場所
    List<GrowPoint> spiritsData = new List<GrowPoint>();

    enum Demon_TYPE
    {
        POPO,
        PUPU,
        PIPI
    }
    //スマホからのメッセージ一覧
    struct SmaphoMsg
    {
        public string order;
        public Demon_TYPE type;
        public int G_HP;
        public int G_ATK;
        public int G_SPD;
        public int G_ATKTime;
    };
    List<SmaphoMsg> smaphoMsgList = new List<SmaphoMsg>();
    
    //スマホ側から受け取る情報を抑制するためのカウンター
    int counter = 0;

    //各種悪魔のプレファブ
    Dictionary<Demon_TYPE, GameObject> demons = new Dictionary<Demon_TYPE, GameObject>(){
        {Demon_TYPE.POPO, null},
        {Demon_TYPE.PUPU, null},
        {Demon_TYPE.PIPI, null}
    };

    //悪魔別指示クラス
    Dictionary<Demon_TYPE, DemonsOrder> orders = new Dictionary<Demon_TYPE, DemonsOrder>(){
        {Demon_TYPE.POPO, new DemonsOrder()},
        {Demon_TYPE.PUPU, new DemonsOrder()},
        {Demon_TYPE.PIPI, new DemonsOrder()}
    };

    [SerializeField, TooltipAttribute("出撃位置")]
    Transform spawnPoint;

    [SerializeField, Range(0.0f, 1.0f), TooltipAttribute("強化時のスケール倍率")]
    float powerUpScale = 0.1f;

    #endregion

    void Start()
    {
        //悪魔達をセットし忘れていたときは検索して取得する
        //速度に困ったら代入方法を変える
        if (demons[Demon_TYPE.POPO] == null)
            demons[Demon_TYPE.POPO] = (GameObject)Resources.Load("Prefabs/Unit/POPO");
        if (demons[Demon_TYPE.PUPU] == null)
            demons[Demon_TYPE.PUPU] = (GameObject)Resources.Load("Prefabs/Unit/PUPU");
        if (demons[Demon_TYPE.PIPI] == null)
            demons[Demon_TYPE.PIPI] = (GameObject)Resources.Load("Prefabs/Unit/PIPI");

        if (spawnPoint == null)
            spawnPoint = new GameObject().transform;
    }

    void FixedUpdate()
    {
        counter++;

        if (counter > 60)
        {
            counter = 0;

            //受信後の処理
            Receive();
        }

        //リストの初めのやつから処理を行う
        if (smaphoMsgList.Count > 0)
        {
            //悪魔別指示クラス
            DemonsOrder order = orders[smaphoMsgList[0].type];
            switch (smaphoMsgList[0].order)
            {
                case "Summon":
                    SummonOrder();
                    break;
                case "Atack_Castle":
                    order.ChangeOrder(DemonsOrder.Order.Castle);
                    break;
                case "Atack_House":
                    order.ChangeOrder(DemonsOrder.Order.Building);
                    break;
                case "Atack_Soldier":
                    order.ChangeOrder(DemonsOrder.Order.Enemy);
                    break;
                case "Get_Spirit":
                    order.ChangeOrder(DemonsOrder.Order.Spirit);
                    break;
            }

            //処理したらリストから外す
            smaphoMsgList.Remove(smaphoMsgList[0]);
        }

        //スピリットデータの数が1個以上ある場合の処理
        if(spiritsData.Count > 0)
        {
            PushSpirit(spiritsData[0]);

            spiritsData.Remove(spiritsData[0]);
        }
    }

    //魂リストへの追加
    public void AddSpiritList(GrowPoint spiritdata)
    {
        spiritsData.Add(spiritdata);
    }

    //魂をサーバーに送信
    void PushSpirit(GrowPoint _spiritData)
    {
        NCMBObject spiritObj = new NCMBObject("SpiritData");

        spiritObj["TYPE"] = _spiritData.GetDemonType.ToString();

        spiritObj.SaveAsync();

    }

    //受信したときの処理
    void Receive()
    {
        //クエリを作成
        NCMBQuery<NCMBObject> demonStatus = new NCMBQuery<NCMBObject>("DemonData");

        //PlayerNoが記入されていないもの以外を取得
        //demonStatus.WhereNotEqualTo("PlayerNo", "");

        //createDateを降順にしてリミットを1に制限することで最新のもののみ取得
        //demonStatus.OrderByDescending("createDate");
        //demonStatus.Limit = 1;

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
                    SmaphoMsg smaphoMsg = new SmaphoMsg();
                    //スマホ側から受け取ったメッセージを変換して登録する
                    if (ncmbObj["Order"] != null)
                        smaphoMsg.order = ncmbObj["Order"].ToString();
                    else
                        smaphoMsg.order = "";

                    //タイプの振り分け
                    Demon_TYPE type = Demon_TYPE.POPO;
                    switch (ncmbObj["Type"].ToString())
                    {
                        case "POPO":
                            type = Demon_TYPE.POPO;
                            break;
                        case "PUPU":
                            type = Demon_TYPE.PUPU;
                            break;
                        case "PIPI":
                            type = Demon_TYPE.PIPI;
                            break;
                        default:
                            Debug.Log("Player.cs Receive() ncmbObj[Type] Exception");
                            break;
                    }
                    smaphoMsg.type = type;

                    smaphoMsg.G_HP = 0;
                    smaphoMsg.G_ATK = 0;
                    smaphoMsg.G_SPD = 0;
                    smaphoMsg.G_ATKTime = 1;

                    //orderがSummonでないとステータスが設定できない
                    if (smaphoMsg.order == "Summon")
                    {
                        if (ncmbObj["HP"] != null)
                            smaphoMsg.G_HP = System.Convert.ToInt32(ncmbObj["HP"].ToString());

                        if (ncmbObj["ATK"] != null)
                            smaphoMsg.G_ATK = System.Convert.ToInt32(ncmbObj["ATK"].ToString());

                        if (ncmbObj["DEX"] != null)
                            smaphoMsg.G_SPD = System.Convert.ToInt32(ncmbObj["DEX"].ToString());

                        //smaphoMsg.G_ATKTime = System.Convert.ToInt32(ncmbObj["ATKTime"].ToString());
                    }

                    smaphoMsgList.Add(smaphoMsg);
                    
                    //記録を取ったら消す
                    ncmbObj.DeleteAsync();
                }
            }
        });
    }

    //召喚指示を受け取った時の処理
    void SummonOrder()
    {
        //出撃座標
        Vector3 spawnPosition = spawnPoint.position;
        //適当な値を入れて重なることを避ける
        Vector3 randVac = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

        //プレファブの悪魔のデータの設定
        //悪魔が作られてからやるSetStatusは個別のステータス
        //おそらくアドレス渡しになるため同じプレファブから作られたすべてのオブジェクトが共通の値を持つこととなる
        GameObject demon = demons[smaphoMsgList[0].type];
        GrowPoint growPoint = demon.GetComponent<Demons>().GrowPoint;

        //成長値の代入
        growPoint.CurrentHP_GrowPoint = smaphoMsgList[0].G_HP;
        growPoint.CurrentATK_GrowPoint = smaphoMsgList[0].G_ATK;
        growPoint.CurrentSPEED_GrowPoint = smaphoMsgList[0].G_SPD;
        growPoint.CurrentAtackTime_GrowPoint = smaphoMsgList[0].G_ATKTime;

        //ステータスの代入
        demon.GetComponent<Demons>().SetStatus();

        //悪魔を出す
        GameObject instaceObject = (GameObject)Instantiate(demon,
                                                        spawnPosition + randVac,           //プレイヤーごとの出撃位置
                                                        Quaternion.identity);
        instaceObject.transform.SetParent(this.transform, false);
        instaceObject.GetComponent<Demons>().Order = orders[smaphoMsgList[0].type];
        instaceObject.GetComponent<Demons>().GrowPoint = growPoint;

        //強さに応じてスケールを変える処理
        float growScale = 1.0f + ((float)growPoint.GetCost() - 1.0f) * powerUpScale;
        //制限
        if (growScale >= 3.0f)
            growScale = 3.0f;
        instaceObject.transform.localScale = new Vector3(growScale, growScale, growScale);
    }


}
