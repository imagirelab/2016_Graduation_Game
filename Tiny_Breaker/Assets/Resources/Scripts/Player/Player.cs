using UnityEngine;
using System.Collections.Generic;
using NCMB;

//大きい画面でのプレイヤークラス(スマホの情報の受け渡しとかプレイヤー番号とか送受信の解析とか)
public class Player : MonoBehaviour
{
    #region フィールド

    //受信が必要ないときにOFFにする
    [SerializeField]
    bool IsReceive = true;
    //送信が必要ないときにOFFにする
    [SerializeField]
    bool IsPush = true;

    //プレイヤーを識別する番号
    //誰の悪魔が魂の回収を行ったのかを判断するため必要だと思った
    public int playerID = 1;
    public int targetID = 2;

    //スマホ側に送信するのに一時的にデータをためておく場所
    List<int> costData = new List<int>();
    List<GrowPoint> spiritsData = new List<GrowPoint>();
    List<GrowPoint> spiritsDataCopy = new List<GrowPoint>();    //デバッグ用に使う魂データのコピー
    
    enum Demon_TYPE
    {
        POPO,
        PUPU,
        PIPI
    }

    enum Direction_TYPE
    {
        Top,
        Middle,
        Bottom
    }

    //スマホからのメッセージ一覧
    struct SmaphoMsg
    {
        public Direction_TYPE dirType;
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

    //各種成長値
    Dictionary<GrowPoint.Type, GrowPoint> growPoints = new Dictionary<GrowPoint.Type, GrowPoint>(){
        {GrowPoint.Type.POPO, null},
        {GrowPoint.Type.PUPU, null},
        {GrowPoint.Type.PIPI, null}
    };
    public GrowPoint PlayerGrowPoint(GrowPoint.Type type) { return growPoints[type]; }


    [SerializeField, TooltipAttribute("出撃位置")]
    Transform spawnPoint;
    public Transform SpawnPoint { get { return spawnPoint; } }

    [SerializeField, Range(0.0f, 1.0f), TooltipAttribute("強化時のスケール倍率")]
    float powerUpScale = 0.1f;

    //巡回ルート配列
    [SerializeField]
    GameObject[] rootes;    //一つ目は出現位置。あと巡回ルート
    Dictionary<int, List<Transform>> rootPointes = new Dictionary<int, List<Transform>>();

    //最終目標物
    [SerializeField]
    public GameObject target;
    //相手のタグ
    [SerializeField]
    string tergetTag = "";
    public string TergetTag { get { return tergetTag; } }

    //召喚する道番号
    int rootNum = 0;

    #endregion

    void Start()
    {
        //悪魔のプレファブの内部数値の成長値だけ初期化
        //速度に困ったら代入方法を変える
        if (demons[Demon_TYPE.POPO] == null)
        {
            demons[Demon_TYPE.POPO] = (GameObject)Resources.Load("Prefabs/Demon/POPO");
            demons[Demon_TYPE.POPO].GetComponent<Demons>().GrowPoint.SetGrowPoint();
            demons[Demon_TYPE.POPO].GetComponent<Demons>().status.SetStatus();
        }
        if (demons[Demon_TYPE.PUPU] == null)
        {
            demons[Demon_TYPE.PUPU] = (GameObject)Resources.Load("Prefabs/Demon/PUPU");
            demons[Demon_TYPE.PUPU].GetComponent<Demons>().GrowPoint.SetGrowPoint();
            demons[Demon_TYPE.PUPU].GetComponent<Demons>().status.SetStatus();
        }
        if (demons[Demon_TYPE.PIPI] == null)
        { 
            demons[Demon_TYPE.PIPI] = (GameObject)Resources.Load("Prefabs/Demon/PIPI");
            demons[Demon_TYPE.PIPI].GetComponent<Demons>().GrowPoint.SetGrowPoint();
            demons[Demon_TYPE.PIPI].GetComponent<Demons>().status.SetStatus();
        }

        //成長値の初期化
        if (growPoints[GrowPoint.Type.POPO] == null)
        {
            growPoints[GrowPoint.Type.POPO] = new GrowPoint();
            growPoints[GrowPoint.Type.POPO].SetDefault(
                GrowPoint.Type.POPO,
                demons[Demon_TYPE.POPO].GetComponent<Demons>().GrowPoint.GetHP_GrowPoint,
                demons[Demon_TYPE.POPO].GetComponent<Demons>().GrowPoint.GetATK_GrowPoint,
                demons[Demon_TYPE.POPO].GetComponent<Demons>().GrowPoint.GetSPEED_GrowPoint,
                demons[Demon_TYPE.POPO].GetComponent<Demons>().GrowPoint.GetAtackTime_GrowPoint);
            growPoints[GrowPoint.Type.POPO].SetGrowPoint();
        }
        if (growPoints[GrowPoint.Type.PUPU] == null)
        {
            growPoints[GrowPoint.Type.PUPU] = new GrowPoint();
            growPoints[GrowPoint.Type.PUPU].SetDefault(
                GrowPoint.Type.PUPU,
                demons[Demon_TYPE.PUPU].GetComponent<Demons>().GrowPoint.GetHP_GrowPoint,
                demons[Demon_TYPE.PUPU].GetComponent<Demons>().GrowPoint.GetATK_GrowPoint,
                demons[Demon_TYPE.PUPU].GetComponent<Demons>().GrowPoint.GetSPEED_GrowPoint,
                demons[Demon_TYPE.PUPU].GetComponent<Demons>().GrowPoint.GetAtackTime_GrowPoint);
            growPoints[GrowPoint.Type.PUPU].SetGrowPoint();
        }
        if (growPoints[GrowPoint.Type.PIPI] == null)
        {
            growPoints[GrowPoint.Type.PIPI] = new GrowPoint();
            growPoints[GrowPoint.Type.PIPI].SetDefault(
                GrowPoint.Type.PIPI,
                demons[Demon_TYPE.PIPI].GetComponent<Demons>().GrowPoint.GetHP_GrowPoint,
                demons[Demon_TYPE.PIPI].GetComponent<Demons>().GrowPoint.GetATK_GrowPoint,
                demons[Demon_TYPE.PIPI].GetComponent<Demons>().GrowPoint.GetSPEED_GrowPoint,
                demons[Demon_TYPE.PIPI].GetComponent<Demons>().GrowPoint.GetAtackTime_GrowPoint);
            growPoints[GrowPoint.Type.PIPI].SetGrowPoint();
        }

        //設定し忘れたときは今いる場所を設定
        if (rootes == null)
            rootes = new GameObject[] { transform.gameObject };
        if (target == null)
            target = transform.gameObject;

        //巡回ルートの作成
        for (int i = 0; i < rootes.Length; i++)
        {
            rootPointes.Add(i, new List<Transform>());

            foreach (Transform child in rootes[i].transform)
                rootPointes[i].Add(child);
            rootPointes[i].Add(target.transform); //最後に最終目的地
        }

        rootNum = 0;

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
            if(IsReceive)   //trueの時だけ受信する
                Receive();
        }

        //リストの初めのやつから処理を行う
        if (smaphoMsgList.Count > 0)
        {
            //方向指示召喚
            switch (smaphoMsgList[0].dirType)
            {
                case Direction_TYPE.Top:
                    ChangeRoot(2);
                    DebugSummon(demons[smaphoMsgList[0].type]);
                    break;
                case Direction_TYPE.Middle:
                    ChangeRoot(1);
                    DebugSummon(demons[smaphoMsgList[0].type]);
                    break;
                case Direction_TYPE.Bottom:
                    ChangeRoot(0);
                    DebugSummon(demons[smaphoMsgList[0].type]);
                    break;
                default:
                    break;
            }
            //処理したらリストから外す
            smaphoMsgList.Remove(smaphoMsgList[0]);
        }

        //スピリットデータの送信
        if (spiritsData.Count > 0)
        {
            //送信がいらないときはしない
            if(IsPush)
                PushSpirit(spiritsData[0]);

            spiritsData.Remove(spiritsData[0]);
        }

        //コストデータの送信
        if (costData.Count > 0)
        {
            //送信がいらないときはしない
            if (IsPush)
                PushCost(costData[0]);

            costData.Remove(costData[0]);
        }
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

                    //方向
                    Direction_TYPE dir = Direction_TYPE.Middle;
                    switch (ncmbObj["Direction"].ToString())
                    {
                        case "Top":
                            dir = Direction_TYPE.Top;
                            break;
                        case "Middle":
                            dir = Direction_TYPE.Middle;
                            break;
                        case "Bottom":
                            dir = Direction_TYPE.Bottom;
                            break;
                        default:
                            Debug.Log("Player.cs Receive() ncmbObj[Direction] Exception");
                            break;
                    }
                    smaphoMsg.dirType = dir;
                    
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
                    
                    if (ncmbObj["HP"] != null)
                        smaphoMsg.G_HP = System.Convert.ToInt32(ncmbObj["HP"].ToString());
                    if (ncmbObj["ATK"] != null)
                        smaphoMsg.G_ATK = System.Convert.ToInt32(ncmbObj["ATK"].ToString());
                    if (ncmbObj["DEX"] != null)
                        smaphoMsg.G_SPD = System.Convert.ToInt32(ncmbObj["DEX"].ToString());
                    //smaphoMsg.G_ATKTime = System.Convert.ToInt32(ncmbObj["ATKTime"].ToString());

                    smaphoMsgList.Add(smaphoMsg);

                    //記録を取ったら消す
                    ncmbObj.DeleteAsync();
                }
            }
        });
    }
    
    //魂をサーバーに送信
    void PushSpirit(GrowPoint _spiritData)
    {
        NCMBObject nbcObj = new NCMBObject("SpiritData");

        nbcObj["TYPE"] = _spiritData.GetDemonType.ToString();

        nbcObj.SaveAsync();
    }

    //コストをサーバーに送信
    void PushCost(int _costData)
    {
        NCMBObject nbcObj = new NCMBObject("CostData");

        nbcObj["Cost"] = _costData.ToString();

        nbcObj.SaveAsync();
    }

    //魂リストへの追加
    public void AddSpiritList(GrowPoint spiritdata)
    {
        spiritsData.Add(spiritdata);

        //コピーをとる
        if (spiritsDataCopy.Count < 10)
            spiritsDataCopy.Add(spiritdata);
    }

    //コストリストへの追加
    public void AddCostList(int costdata)
    {
        costData.Add(costdata);
        
        //デバック表示用
        PlayerCost playerCost = gameObject.GetComponent<PlayerCost>();
        playerCost.AddCost(costdata);
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
        //demon.GetComponent<Demons>().SetStatus();

        //悪魔を出す
        GameObject instaceObject = (GameObject)Instantiate(demon,
                                                        spawnPosition + randVac,           //プレイヤーごとの出撃位置
                                                        Quaternion.identity);
        instaceObject.transform.SetParent(this.transform, false);
        //instaceObject.GetComponent<Demons>().Order = orders[smaphoMsgList[0].type];
        instaceObject.GetComponent<Demons>().GrowPoint = growPoint;

        //強さに応じてスケールを変える処理
        float growScale = 1.0f + ((float)growPoint.GetCost() - 1.0f) * powerUpScale;
        //制限
        if (growScale >= 3.0f)
            growScale = 3.0f;
        instaceObject.transform.localScale = new Vector3(growScale, growScale, growScale);
    }

    public void DebugPowerUP(GameObject demon)
    {
        if (spiritsDataCopy.Count == 0)
            return;

        //GrowPoint demonPoint = demon.GetComponent<Demons>().GrowPoint;
        GrowPoint demonPoint = growPoints[demon.GetComponent<Demons>().GrowPoint.GetDemonType];
        GrowPoint spiritPoint = spiritsDataCopy[0];

        demonPoint.AddGrowPoint(spiritPoint);
        //demonPoint.CurrentHP_GrowPoint += demonPoint.GetHP_GrowPoint + spiritPoint.GetHP_GrowPoint;
        //demonPoint.CurrentATK_GrowPoint += demonPoint.GetATK_GrowPoint + spiritPoint.GetATK_GrowPoint;
        //demonPoint.CurrentSPEED_GrowPoint += demonPoint.GetSPEED_GrowPoint + spiritPoint.GetSPEED_GrowPoint;
        //demonPoint.CurrentAtackTime_GrowPoint += demonPoint.GetAtackTime_GrowPoint + spiritPoint.GetAtackTime_GrowPoint;

        demon.GetComponent<Demons>().GrowPoint = demonPoint;

        //ステータスの代入  あってもなくてもいい　デバック用の表示更新用
        //demon.GetComponent<Demons>().SetStatus();

        spiritsDataCopy.Remove(spiritsDataCopy[0]);
    }

    public void ChangeRoot(int rootNum)
    {
        this.rootNum = rootNum;
    }

    public void DebugSummon(GameObject demon)
    {
        //適当な値を入れて重なることを避ける
        Vector3 randVac = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

        //プレファブの悪魔のデータの設定
        //悪魔が作られてからやるSetStatusは個別のステータス
        //おそらくアドレス渡しになるため同じプレファブから作られたすべてのオブジェクトが共通の値を持つこととなる
        //GrowPoint growPoint = demon.GetComponent<Demons>().GrowPoint;
        GrowPoint growPoint = growPoints[demon.GetComponent<Demons>().GrowPoint.GetDemonType];

        //召喚コストの計算
        int demonCost = GetComponent<PlayerCost>().GetCurrentDemonCost(demon.GetComponent<Demons>().GrowPoint.Level);
        
        if (GetComponent<PlayerCost>().UseableCost(demonCost))
        {
            //悪魔を出す
            GameObject instaceObject = (GameObject)Instantiate(demon,
                                                            rootes[rootNum].transform.position + randVac,
                                                            Quaternion.identity);
            instaceObject.transform.SetParent(this.transform, false);
            Vector3 summonVec = (rootPointes[rootNum].ToArray()[0].position - rootes[rootNum].transform.position).normalized;   //初めの向き
            Quaternion rotation = Quaternion.LookRotation(summonVec);
            instaceObject.transform.rotation = rotation;
            instaceObject.tag = transform.gameObject.tag;    //自分のタグを設定
            instaceObject.layer = transform.gameObject.layer;    //レイヤーを設定
            instaceObject.GetComponent<Unit>().targetTag = tergetTag;   //相手のタグを設定
            instaceObject.GetComponent<Unit>().goalObject = target; //最終目標
                                                                    //instaceObject.GetComponent<Demons>().Order = orders[Demon_TYPE.PUPU];
            instaceObject.GetComponent<Demons>().GrowPoint = growPoint;
            instaceObject.GetComponent<Demons>().LoiteringPointObj = rootPointes[rootNum].ToArray();

            //出るとき重なる瞬間は回らないように
            instaceObject.GetComponent<Rigidbody>().freezeRotation = true;

            //強さに応じてスケールを変える処理
            //float growScale = demon.transform.localScale.magnitude + ((float)growPoint.GetCost() - 1.0f) * powerUpScale;
            ////制限
            //if (growScale >= 4.0f)
            //    growScale = 4.0f;
            //instaceObject.transform.localScale = new Vector3(growScale, growScale, growScale);
        }
    }

    public GrowPoint GetFirstSpirit()
    {
        if (spiritsDataCopy.Count == 0)
            return null;
        else
            return spiritsDataCopy[0];
    }
}
