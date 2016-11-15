using UnityEngine;
using System.Collections.Generic;
using NCMB;

//大きい画面でのプレイヤークラス(スマホの情報の受け渡しとかプレイヤー番号とか送受信の解析とか)
public class Player : MonoBehaviour
{
    #region フィールド
    
    //送信が必要ないときにOFFにする
    [SerializeField]
    bool IsPush = true;

    //プレイヤーを識別する番号
    //誰の悪魔が魂の回収を行ったのかを判断するため必要だと思った
    public int playerID = 0;
    public int targetID = 1;

    //スマホ側に送信するのに一時的にデータをためておく場所
    List<int> costData = new List<int>();
    List<Enum.Demon_TYPE> spiritsData = new List<Enum.Demon_TYPE>();
    List<Enum.Demon_TYPE> spiritsDataCopy = new List<Enum.Demon_TYPE>();    //デバッグ用に使う魂データのコピー
    
    //各種悪魔のプレファブ
    [SerializeField]
    GameObject[] demons = new GameObject[(int)Enum.Demon_TYPE.Num];
    public GameObject[] Demons { get { return demons; } }
    //Playerクラス内で使う悪魔達のレベル
    int[] demonsLevel = new int[(int)Enum.Demon_TYPE.Num];
    public int[] DemonsLevel { get { return demonsLevel; } set { demonsLevel = value; } }
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
    
    [SerializeField]
    int maxSummonNum = 50;

    [SerializeField]
    GameObject deathbolwObj;

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
        
        if (spawnPoint == null)
            spawnPoint = new GameObject().transform;

        if (deathbolwObj == null)
            deathbolwObj = new GameObject();
    }

    void Update()
    {
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
    
    //魂をサーバーに送信
    void PushSpirit(Enum.Demon_TYPE _spiritData)
    {
        NCMBObject nbcObj = new NCMBObject("SpiritData");

        nbcObj["TYPE"] = _spiritData.ToString();
        nbcObj["PlayerNo"] = playerID.ToString();

        nbcObj.SaveAsync();
    }

    //コストをサーバーに送信
    void PushCost(int _costData)
    {
        NCMBObject nbcObj = new NCMBObject("CostData");

        nbcObj["Cost"] = _costData.ToString();
        nbcObj["PlayerNo"] = playerID.ToString();

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
    //正しく召喚されたらtrueを返す
    public void DebugSummon(int demonType)
    {
        //生成した悪魔の数のカウント
        int childDemonsCount = 0;
        foreach (Transform child in transform)
            if (child.GetComponent<Demons>())
                childDemonsCount++;

        //召喚できない条件なら何もしないで返す
        if (childDemonsCount > maxSummonNum)
            return;
        
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
            GameObject instace = (GameObject)Instantiate(demons[demonType],
                                                            spawnPoint.position,
                                                            Quaternion.identity);
            //レベル上げ
            instace.GetComponent<Unit>().status.SetStatus(demonsLevel[demonType]);
            instace.transform.SetParent(this.transform, false);   //親を出したプレイヤーに設定
            Vector3 summonVec = (rootPointes[(int)rootNum].ToArray()[0].position - rootes[(int)rootNum].transform.position).normalized;   //初めの向き
            Quaternion rotation = Quaternion.LookRotation(summonVec);
            instace.transform.rotation = rotation;    //出た瞬間の向き
            instace.tag = transform.gameObject.tag;    //自分のタグを設定
            instace.layer = transform.gameObject.layer;    //レイヤーを設定
            instace.GetComponent<Unit>().targetTag = tergetTag;   //相手のタグを設定
            instace.GetComponent<Unit>().goalObject = target; //最終目標
            instace.GetComponent<Unit>().LoiteringPointObj = rootPointes[(int)rootNum].ToArray();  //巡回ルート地点配列
            instace.GetComponent<Unit>().rootNum = rootNum;   //ルート番号
            instace.GetComponent<Unit>().SpawnTargetPosition = rootes[(int)rootNum].transform.position + randVac;

            //出るとき重なる瞬間は回らないように
            instace.GetComponent<Rigidbody>().freezeRotation = true;

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
    
    //悪魔のレベルを設定
    public void SetDemonLevel(int demonType, int level)
    {
        ////レベル上限
        //if (level >= 20)
        //    return;

        demonsLevel[demonType] = level;
    }

    //必殺技の発動
    public void Deathblow()
    {
        GameObject instace = (GameObject)Instantiate(deathbolwObj,
                                                    spawnPoint.position,
                                                    Quaternion.identity);
        instace.GetComponent<Missile>().Target = target;
        instace.GetComponent<Missile>().enabled = true;
    }
}
