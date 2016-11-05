using UnityEngine;
using System.Collections;
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
    List<Enum.Demon_TYPE> spiritsData = new List<Enum.Demon_TYPE>();
    List<Enum.Demon_TYPE> spiritsDataCopy = new List<Enum.Demon_TYPE>();    //デバッグ用に使う魂データのコピー
    
    //スマホからのメッセージ一覧
    struct SmaphoMsg
    {
        public Enum.Direction_TYPE dirType;
        public Enum.Demon_TYPE type;
        public int G_HP;
        public int G_ATK;
        public int G_SPD;
        public int G_ATKTime;
    };
    List<SmaphoMsg> smaphoMsgList = new List<SmaphoMsg>();
    
    //スマホ側から受け取る情報を抑制するためのカウンター
    int counter = 0;

    //各種悪魔のプレファブ
    [SerializeField]
    GameObject[] demons = new GameObject[(int)Enum.Demon_TYPE.Num];
    public GameObject[] Demons { get { return demons; } }
    //Playerクラス内で使う悪魔達のレベル
    int[] demonsLevel = new int[(int)Enum.Demon_TYPE.Num];
    public int[] DemonsLevel { get { return demonsLevel; } }
    //Debug確認用
    Status[] demonsStatus = new Status[(int)Enum.Demon_TYPE.Num];
    public Status[] DemonsStatus { get { return demonsStatus; } }

    [SerializeField, TooltipAttribute("出撃位置")]
    Transform spawnPoint;
    public Transform SpawnPoint { get { return spawnPoint; } }

    //巡回ルート配列
    [SerializeField]
    GameObject[] rootes;    //一つ目は出現位置。あと巡回ルート
    Dictionary<int, List<Transform>> rootPointes = new Dictionary<int, List<Transform>>();

    //最終目標物
    public GameObject target;
    //相手のタグ
    [SerializeField]
    string tergetTag = "";
    public string TergetTag { get { return tergetTag; } }

    //召喚する道番号 ０：下　１：真ん中　２：上
    Enum.Direction_TYPE rootNum = Enum.Direction_TYPE.Bottom;

    //召喚クールタイム
    [SerializeField]
    float coolTime = 0.5f;
    bool IsCool = false;

    [SerializeField]
    int maxSummonNum = 50;

    #endregion

    void Start()
    {
        //悪魔のレベルだけ初期化
        for(int i = 0; i < demonsLevel.Length; i++)
            demonsLevel[i] = 0;
        //ステータスの作成
        for (int i = 0; i < demonsStatus.Length; i++)
        {
            Status status = demons[i].GetComponent<Unit>().status;
            demonsStatus[i] = new Status();
            demonsStatus[i].SetDefault(status.GetHP, status.GetATK, status.GetSPEED, status.GetAtackTime);
            demonsStatus[i].SetStatus(demonsLevel[i]);
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
        
        //クールカウントの初期化
        IsCool = false;

        if (spawnPoint == null)
            spawnPoint = new GameObject().transform;
    }

    void FixedUpdate()
    {
        //受信制御
        counter++;

        if (counter > 30)
        {
            counter = 0;

            //受信後の処理
            if(IsReceive)   //trueの時だけ受信する
                Receive();
        }


        //リストの初めのやつから処理を行う
        if (smaphoMsgList.Count > 0)
        {
            //方向指示
            ChangeRoot((int)smaphoMsgList[0].dirType);
            //召喚
            DebugSummon((int)smaphoMsgList[0].type);
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

        //自分のPlayerNoが記入されていないもの以外を取得
        demonStatus.WhereEqualTo("PlayerNo", playerID);

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
                    Enum.Direction_TYPE dir = Enum.Direction_TYPE.Middle;
                    switch (ncmbObj["Direction"].ToString())
                    {
                        case "Top":
                            dir = Enum.Direction_TYPE.Top;
                            break;
                        case "Middle":
                            dir = Enum.Direction_TYPE.Middle;
                            break;
                        case "Bottom":
                            dir = Enum.Direction_TYPE.Bottom;
                            break;
                        default:
                            Debug.Log("Player.cs Receive() ncmbObj[Direction] Exception");
                            break;
                    }
                    smaphoMsg.dirType = dir;

                    //タイプの振り分け
                    Enum.Demon_TYPE type = Enum.Demon_TYPE.POPO;
                    switch (ncmbObj["Type"].ToString())
                    {
                        case "POPO":
                            type = Enum.Demon_TYPE.POPO;
                            break;
                        case "PUPU":
                            type = Enum.Demon_TYPE.PUPU;
                            break;
                        case "PIPI":
                            type = Enum.Demon_TYPE.PIPI;
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
                    if (ncmbObj["SPEED"] != null)
                        smaphoMsg.G_SPD = System.Convert.ToInt32(ncmbObj["SPEED"].ToString());
                    //smaphoMsg.G_ATKTime = System.Convert.ToInt32(ncmbObj["ATKTime"].ToString());

                    smaphoMsgList.Add(smaphoMsg);

                    //記録を取ったら消す
                    ncmbObj.DeleteAsync();
                }
            }
        });
    }
    
    //魂をサーバーに送信
    void PushSpirit(Enum.Demon_TYPE _spiritData)
    {
        NCMBObject nbcObj = new NCMBObject("SpiritData");

        nbcObj["TYPE"] = _spiritData.ToString();
        nbcObj["PlayerNo"] = playerID;

        nbcObj.SaveAsync();
    }

    //コストをサーバーに送信
    void PushCost(int _costData)
    {
        NCMBObject nbcObj = new NCMBObject("CostData");

        nbcObj["Cost"] = _costData;
        nbcObj["PlayerNo"] = playerID;

        nbcObj.SaveAsync();
    }

    //魂リストへの追加
    public void AddSpiritList(Enum.Demon_TYPE spiritdata)
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
    
    //パワーアップ
    public void DebugPowerUP(int demonType)
    {
        //魂がなければそのまま返す
        if (spiritsDataCopy.Count == 0)
            return;
        
        //レベル上限
        if (demonsLevel[demonType] >= 20)
            return;

        demonsLevel[demonType]++;

        spiritsDataCopy.Remove(spiritsDataCopy[0]);

        //デバック確認用
        demonsStatus[demonType].SetStatus(demonsLevel[demonType]);
    }

    //進む方向変える（ボタンで呼び出す関数なので引数にint型で方向を指示）
    public void ChangeRoot(int dir)
    {
        this.rootNum = (Enum.Direction_TYPE)dir;
    }

    //召喚指示（ボタンで呼び出す関数なので引数にint型で召喚する種類を指示）
    public void DebugSummon(int demonType)
    {
        //生成した悪魔の数のカウント
        int childDemonsCount = 0;
        foreach (Transform child in transform)
            if (child.GetComponent<Demons>())
                childDemonsCount++;

        //召喚できない条件なら何もしないで返す
        if (IsCool || childDemonsCount > maxSummonNum)
            return;

        //召喚と同時にクールタイムに入る
        StartCoroutine(CoolTime());

        //適当な値を入れて重なることを避ける
        Vector3 randVac;
        switch(rootNum)
        {
            case Enum.Direction_TYPE.Bottom:
                randVac = new Vector3(Random.Range(-5.5f, 5.5f), 0.0f, Random.Range(-4.0f, 10.0f));
                break;
            case Enum.Direction_TYPE.Middle:
                randVac = new Vector3(Random.Range(-5.0f, 5.0f), 0.0f, Random.Range(-9.0f, 9.0f));
                break;
            case Enum.Direction_TYPE.Top:
                randVac = new Vector3(Random.Range(-6.0f, 6.0f), 0.0f, Random.Range(-9.0f, 9.0f));
                break;
            default:
                randVac = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
                break;
        }
        
        //召喚コストの計算
        int demonCost = GetComponent<PlayerCost>().GetCurrentDemonCost(demonsLevel[demonType]);
        
        if (GetComponent<PlayerCost>().UseableCost(demonCost))
        {
            //悪魔を出す
            GameObject instaceObject = (GameObject)Instantiate(demons[demonType],
                                                            spawnPoint.position,
                                                            Quaternion.identity);
            instaceObject.GetComponent<Unit>().status.SetStatus(demonsLevel[demonType]);
            instaceObject.transform.SetParent(this.transform, false);   //親を出したプレイヤーに設定
            Vector3 summonVec = (rootPointes[(int)rootNum].ToArray()[0].position - rootes[(int)rootNum].transform.position).normalized;   //初めの向き
            Quaternion rotation = Quaternion.LookRotation(summonVec);
            instaceObject.transform.rotation = rotation;    //出た瞬間の向き
            instaceObject.tag = transform.gameObject.tag;    //自分のタグを設定
            instaceObject.layer = transform.gameObject.layer;    //レイヤーを設定
            instaceObject.GetComponent<Unit>().targetTag = tergetTag;   //相手のタグを設定
            instaceObject.GetComponent<Unit>().goalObject = target; //最終目標
            instaceObject.GetComponent<Unit>().LoiteringPointObj = rootPointes[(int)rootNum].ToArray();  //巡回ルート地点配列
            instaceObject.GetComponent<Unit>().rootNum = rootNum;   //ルート番号
            instaceObject.GetComponent<Unit>().SpawnTargetPosition = rootes[(int)rootNum].transform.position + randVac;

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

    //魂リストの最初の種類
    public Enum.Demon_TYPE GetFirstSpirit()
    {
        if (spiritsDataCopy.Count == 0)
            return Enum.Demon_TYPE.None;
        else
            return spiritsDataCopy[0];
    }

    //クールタイムの処理
    IEnumerator CoolTime()
    {
        IsCool = true;

        yield return new WaitForSeconds(coolTime);

        IsCool = false;

        yield return null;
    }
}
