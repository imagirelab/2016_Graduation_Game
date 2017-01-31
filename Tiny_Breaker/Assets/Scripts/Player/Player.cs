using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaticClass;
using SocketIO;

//大きい画面でのプレイヤークラス(スマホの情報の受け渡しとかプレイヤー番号とか送受信の解析とか)
public class Player : MonoBehaviour
{
    #region フィールド

    SocketIOComponent socket;

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
    
    [SerializeField, HeaderAttribute("Deathblow"), TooltipAttribute("回復してから出撃するまでの間隔")]
    float recoveryWaitTime = 1.0f;
    [SerializeField, TooltipAttribute("悪魔軍団の出撃回数")]
    int demonArmyWaveCount = 5;
    //[SerializeField, TooltipAttribute("悪魔軍団のレベル")]
    //int demonArmyLevel = 20;
    [SerializeField, TooltipAttribute("悪魔軍団の出撃間隔")]
    float demonArmyWaveWaitTime = 1.5f;
    [SerializeField, TooltipAttribute("悪魔軍団の１匹ごとの出撃間隔")]
    float demonArmyOneWaitTime = 0.3f;
    
    public Coroutine deathblowcor;

    public Animator potAnimator;

    public Pause pause;
    public GameObject catin;
    public GameObject movie;

    public Main main;

    #endregion
    
    void Start()
    {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        //悪魔のレベルだけ初期化
        demonsLevel[(int)Enum.Demon_TYPE.POPO] = RoundDataBase.getInstance().POPOLevel[playerID - 1];
        demonsLevel[(int)Enum.Demon_TYPE.PUPU] = RoundDataBase.getInstance().PUPULevel[playerID - 1];
        demonsLevel[(int)Enum.Demon_TYPE.PIPI] = RoundDataBase.getInstance().PIPILevel[playerID - 1];

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

        //スタートに得られるコスト
        PlayerCost palyerCost = GetComponent<PlayerCost>();
        AddCostList(palyerCost.GetStateCost);
    }

    void Update()
    {
        //スピリットデータの送信
        if (spiritsData.Count > 0)
        {
            //送信がいらないときはしない
            if (IsPush)
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
        JSONObject jsonObject = new JSONObject(JSONObject.Type.STRING);
        jsonObject.AddField("Type", _spiritData.ToString());
        jsonObject.AddField("PlayerID", playerID - 1);

        socket.Emit("SpiritPush", jsonObject);
    }

    //コストをサーバーに送信
    void PushCost(int _costData)
    {
        JSONObject jsonObject = new JSONObject(JSONObject.Type.STRING);
        jsonObject.AddField("Cost", _costData);
        jsonObject.AddField("PlayerID", playerID - 1);

        socket.Emit("AddCost", jsonObject);
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

        //召喚コストの計算
        PlayerCost palyerCost = GetComponent<PlayerCost>();
        int demonCost = palyerCost.GetCurrentDemonCost(demonsLevel[demonType]);

        //召喚できない条件なら何もしないで返す
        if (childDemonsCount >= maxSummonNum)
        {
            AddCostList(demonCost);
            return;
        }

        //適当な値を入れて重なることを避ける
        Vector3 randVac;
        switch (rootNum)
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
        
        if (GetComponent<PlayerCost>().UseableCost(demonCost))
        {
            //悪魔を出す
            GameObject instace = (GameObject)Instantiate(demons[demonType],
                                                            spawnPoint.position,
                                                            Quaternion.identity);

            //出現SE再生
            SummonSE.SummonSEFlag = true;

            //レベル上げ
            instace.GetComponent<Unit>().status.SetStatus(demonsLevel[demonType]);
            instace.GetComponent<Unit>().level = demonsLevel[demonType];
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
            
            //生まれた数をカウントする
            RoundDataBase.getInstance().AddPassesPopCount(gameObject.tag);

            if (!potAnimator.GetBool("DeathblowFlag"))
            {
                potAnimator.SetTrigger("StopSpawn");
                potAnimator.SetTrigger("Spawn");
            }
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
        demonsLevel[demonType] = level;
    }

    //必殺技の発動
    public void Deathblow()
    {
        deathblowcor = StartCoroutine(SummonDemonArmy());
    }

    //必殺技の発動(デバッグ用)
    public void DebugDeathblow()
    {
        GameObject instace = (GameObject)Instantiate(deathbolwObj,
                                                    spawnPoint.position,
                                                    Quaternion.identity);
        instace.GetComponent<Missile>().Target = target;
        instace.GetComponent<Missile>().enabled = true;
    }

    //終了時の処理
    public IEnumerator ChildDead()
    {
        //必殺技終了
        if (deathblowcor != null)
            StopCoroutine(deathblowcor);

        //出してる悪魔を全て殺す
        foreach (Transform child in transform)
            if (child.gameObject.GetComponent<Unit>())
                child.gameObject.GetComponent<Unit>().status.CurrentHP = 0;

        foreach (GameObject e in FindObjectsOfType(typeof(GameObject)))
        {
            if (e.GetComponent<Unit>())
                if (e.tag == gameObject.tag)
                    e.GetComponent<Unit>().status.CurrentHP = 0;
        }

        yield return null;
    }

    //悪魔軍団の召喚
    IEnumerator SummonDemonArmy()
    {
        while (pause.pausing)
            yield return null;

        //操作不能
        main.StopRequest();

        //ムービーの再生
        Instantiate(catin, catin.transform.position, catin.transform.rotation);
        yield return new WaitForSeconds(1.3f);

        pause.pausing = true;
        movie.SetActive(true);
        yield return null;

        while (pause.pausing)
        {
            if (!movie.GetComponent<Movie_On_UI>().IsPlaying)
            {
                movie.SetActive(false);
                pause.pausing = false;
            }
            yield return null;
        }

        if (!pause.pausing)
            movie.SetActive(false);

        //操作可能
        main.StopEndRequest();

        //ポットモーション
        potAnimator.SetBool("DeathblowFlag", true);

        //コスト全回復
        PlayerCost playerCost = gameObject.GetComponent<PlayerCost>();
        AddCostList(playerCost.GetMaxCost);
        yield return null;
        yield return new WaitForSeconds(recoveryWaitTime);

        //ルート出撃候補
        //一度しか使わない技ということでローカルで宣言
        //ルートの数でパターンを作る
        int[,] rootCandidate = {
            { 0, 1, 2 } ,
            { 0, 2, 1 } ,
            { 1, 0, 2 } ,
            { 1, 2, 0 } ,
            { 2, 0, 1 } ,
            { 2, 1, 0 }
        };

        //出撃する回数分回す
        for (int i = 0; i < demonArmyWaveCount; i++)
        {
            //ルート出撃候補からランダムでパターンを選ぶ
            int rand = Random.Range(0, rootCandidate.GetLength(0) - 1);

            for (int j = 0; j < (int)Enum.Direction_TYPE.Num; j++)
            {
                int randDemonType = Random.Range(0, (int)Enum.Demon_TYPE.Num);

                //適当な値を入れて重なることを避ける
                Vector3 randVac;
                switch ((Enum.Direction_TYPE)rootCandidate[rand, j])
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

                //悪魔を出す
                GameObject instace = (GameObject)Instantiate(demons[randDemonType],
                                                                spawnPoint.position,
                                                                Quaternion.identity);

                //出現SE再生
                SummonSE.SummonSEFlag = true;

                //レベル上げ
                instace.GetComponent<Unit>().status.SetStatus(demonsLevel[randDemonType]);
                instace.GetComponent<Unit>().level = demonsLevel[randDemonType];
                Vector3 summonVec = (rootPointes[rootCandidate[rand, j]].ToArray()[0].position - rootes[rootCandidate[rand, j]].transform.position).normalized;   //初めの向き
                Quaternion rotation = Quaternion.LookRotation(summonVec);
                instace.transform.rotation = rotation;    //出た瞬間の向き
                instace.transform.SetParent(this.transform, false);   //親を出したプレイヤーに設定
                instace.tag = transform.gameObject.tag;    //自分のタグを設定
                instace.layer = transform.gameObject.layer;    //レイヤーを設定
                instace.GetComponent<Unit>().targetTag = tergetTag;   //相手のタグを設定
                instace.GetComponent<Unit>().goalObject = target; //最終目標
                instace.GetComponent<Unit>().LoiteringPointObj = rootPointes[rootCandidate[rand, j]].ToArray();  //巡回ルート地点配列
                instace.GetComponent<Unit>().rootNum = (Enum.Direction_TYPE)rootCandidate[rand, j];   //ルート番号
                instace.GetComponent<Unit>().SpawnTargetPosition = rootes[rootCandidate[rand, j]].transform.position + randVac;

                //出るとき重なる瞬間は回らないように
                instace.GetComponent<Rigidbody>().freezeRotation = true;

                //生まれた数をカウントする
                RoundDataBase.getInstance().AddPassesPopCount(gameObject.tag);

                yield return new WaitForSeconds(demonArmyOneWaitTime);
            }

            yield return new WaitForSeconds(demonArmyWaveWaitTime - demonArmyOneWaitTime * (int)Enum.Direction_TYPE.Num);
        }

        //ポットモーション
        potAnimator.SetBool("DeathblowFlag", false);
    }
}
