using UnityEngine;
using System.Collections;
using System.IO;
using Loader;
using StaticClass;

public class LoadManager : MonoBehaviour
{
    [SerializeField]
    bool IsLoad = true;
    bool loadEnd = false;
    public bool LoadEnd { get { return loadEnd; } }

    private static readonly string paramurl = "https://yoo3006.github.io/ParamData.csv";
    private static readonly string costurl = "https://yoo3006.github.io/CostData.csv";
    private static readonly string huturl = "https://yoo3006.github.io/HutData.csv";

    [SerializeField]
    GameObject prePOPO;
    [SerializeField]
    GameObject prePUPU;
    [SerializeField]
    GameObject prePIPI;
    [SerializeField]
    GameObject preShield;
    [SerializeField]
    GameObject preAx;
    [SerializeField]
    GameObject preGun;

    [SerializeField]
    GameObject[] huts;
    
    public IEnumerator Start()
    {
        loadEnd = false;

        //ロードしない設定なら飛ばす
        if (IsLoad)
        {
            //ゲームオブジェクトの設定しわすれがあった時、
            //メッセージを名前にして空のオブジェクトを作る
            if (prePOPO == null)
                prePOPO = new GameObject(this.ToString() + " prePOPO Null");
            if (prePUPU == null)
                prePUPU = new GameObject(this.ToString() + " prePUPU Null");
            if (prePIPI == null)
                prePIPI = new GameObject(this.ToString() + " prePIPI Null");
            if (preShield == null)
                preShield = new GameObject(this.ToString() + " preShield Null");
            if (preAx == null)
                preAx = new GameObject(this.ToString() + " preAx Null");
            if (preGun == null)
                preGun = new GameObject(this.ToString() + " preGun Null");

            if (huts == null)
                huts = new GameObject[1];


            //gh-pageから文字列を取得
            WWW paramwww = new WWW(paramurl);
            WWW costwww = new WWW(costurl);
            WWW hutwww = new WWW(huturl);

            yield return paramwww;
            yield return costwww;
            yield return hutwww;

            string paramtext = paramwww.text;
            string costtext = costwww.text;
            string huttext = hutwww.text;

            ////プロジェクト内のファイルを取得
            //string paramtext = GetCSVString("/Resources/CSVData/ParamData.csv");
            //string growtext = GetCSVString("/Resources/CSVData/GrowData.csv");
            //string costtext = GetCSVString("/Resources/CSVData/CostData.csv");

            ParamData ParamTable = new ParamData();
            ParamTable.Load(paramtext);

            CostData CostTable = new CostData();
            CostTable.Load(costtext);

            HutData HutTable = new HutData();
            HutTable.Load(huttext);

            //パラメータデータの取り込み
            foreach (var param in ParamTable.All)
            {
                switch (param.ID)
                {
                    case "popo":
                        if (prePOPO != null)
                            SetParm(param, prePOPO);
                        break;
                    case "pupu":
                        if (prePUPU != null)
                            SetParm(param, prePUPU);
                        break;
                    case "pipi":
                        if (prePIPI != null)
                            SetParm(param, prePIPI);
                        break;
                    case "shield":
                        if (preShield != null)
                            SetParm(param, preShield);
                        break;
                    case "ax":
                        if (preAx != null)
                            SetParm(param, preAx);
                        break;
                    case "gun":
                        if (preGun != null)
                            SetParm(param, preGun);
                        break;
                    default:
                        break;
                }
            }

            //コストデータの取り込み
            SetCost(CostTable);

            //小屋データの取り込み
            SetHut(HutTable);

            Debug.Log("Load END");
        }

        loadEnd = true;
    }

    /// <summary>
    ///　CSVファイルの文字列を取得
    /// </summary>
    /// <param name="path">Assetフォルダ以下のCSVファイルの位置を書く</param>
    /// <returns>CSVファイルの文字列</returns>
    string GetCSVString(string path)
    {
        StreamReader sr = new StreamReader(Application.dataPath + path);
        string strStream = sr.ReadToEnd();

        return strStream;
    }

    //パラメータの設定
    void SetParm(ParamMaster param, GameObject unit)
    {
        if (unit.GetComponent<Unit>())
        {
            unit.GetComponent<Unit>().status.SetDefault(param.HP, param.ATK, param.SPEED, param.ATKSPEED);
            unit.GetComponent<Unit>().ATKRange = param.ATKRENGE;
        }
    }

    //コストの設定
    void SetCost(CostData CostTable)
    {
        foreach (var cost in CostTable.All)
        {
            //ラウンド数によってセットする値を変える
            if(cost.Round == GameRule.getInstance().round.Count)
                for(int i = 0; i < GameRule.playerNum; i++)
                {
                    GameObject player = GameObject.Find("Player" + (i + 1));
                    if (player.GetComponent<PlayerCost>())
                        player.GetComponent<PlayerCost>().SetDefault(cost.MaxCost, cost.StateCost, cost.CostParSecond, cost.DemonCost, cost.SoldierCost, cost.HouseCost, cost.ReturnCost);
                }
        }
    }

    //小屋の設定
    void SetHut(HutData hutTable)
    {
        foreach (var hut in hutTable.All)
        {
            if (huts[hut.No].GetComponent<Spawner>())
                huts[hut.No].GetComponent<Spawner>().SetDefault( hut.SpawnNum, hut.SpawnMax, hut.SpawnTime);
            if (huts[hut.No].GetComponent<House>())
                huts[hut.No].GetComponent<House>().SetDefault(hut.HP, hut.Regene, hut.RegeneTime);
        }
    }
}
