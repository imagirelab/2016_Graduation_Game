using UnityEngine;
using System.Collections;
using System.IO;
using Loader;
using StaticClass;

public class LoadManager : MonoBehaviour
{
    [SerializeField]
    bool IsLoad = true;

    private static readonly string paramurl = "https://yoo3006.github.io/ParamData.csv";
    private static readonly string growurl = "https://yoo3006.github.io/GrowData.csv";
    private static readonly string costurl = "https://yoo3006.github.io/CostData.csv";

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

    //void Start()
    public IEnumerator Start()
    {
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

            //gh-pageから文字列を取得
            WWW paramwww = new WWW(paramurl);
            WWW growwww = new WWW(growurl);
            WWW costwww = new WWW(costurl);

            yield return paramwww;
            yield return growwww;
            yield return costwww;

            string paramtext = paramwww.text;
            string growtext = growwww.text;
            string costtext = costwww.text;

            ////プロジェクト内のファイルを取得
            //string paramtext = GetCSVString("/Resources/CSVData/ParamData.csv");
            //string growtext = GetCSVString("/Resources/CSVData/GrowData.csv");
            //string costtext = GetCSVString("/Resources/CSVData/CostData.csv");

            ParamData ParamTable = new ParamData();
            ParamTable.Load(paramtext);

            GrowData GrowTable = new GrowData();
            GrowTable.Load(growtext);

            CostData CostTable = new CostData();
            CostTable.Load(costtext);

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

            foreach (var grow in GrowTable.All)
            {
                switch (grow.ID)
                {
                    case "popo":
                        if (prePOPO != null)
                            SetGrow(grow, prePOPO, GrowPoint.Type.POPO);
                        break;
                    case "pupu":
                        if (prePUPU != null)
                            SetGrow(grow, prePUPU, GrowPoint.Type.PUPU);
                        break;
                    case "pipi":
                        if (prePIPI != null)
                            SetGrow(grow, prePIPI, GrowPoint.Type.PIPI);
                        break;
                    default:
                        break;
                }
            }

            SetCost(CostTable);

            Debug.Log("Load END");
        }
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

    void SetParm(ParamMaster param, GameObject unit)
    {
        if (unit.GetComponent<Unit>())
        {
            unit.GetComponent<Unit>().status.SetDefault(param.HP, param.ATK, param.SPEED, param.ATKSPEED);
            unit.GetComponent<Unit>().ATKRange = param.ATKRENGE;
        }
    }

    void SetGrow(GrowMaster grow, GameObject unit, GrowPoint.Type type)
    {
        if (unit.GetComponent<Demons>())
            unit.GetComponent<Demons>().GrowPoint.SetDefault(type, grow.GHP, grow.GATK, grow.GSPEED, grow.GATKSPEED);
    }

    void SetCost(CostData CostTable)
    {
        foreach (var cost in CostTable.All)
        {
            for(int i = 0; i < GameRule.getInstance().playerNum; i++)
            {
                GameObject player = GameObject.Find("Player" + (i + 1));
                if (player.GetComponent<PlayerCost>())
                    player.GetComponent<PlayerCost>().SetDefault(cost.MaxCost, cost.StateCost, cost.CostParSecond, cost.DemonCost, cost.DemonCostRate, cost.SoldierCost, cost.HouseCost);
            }
        }
    }
}
