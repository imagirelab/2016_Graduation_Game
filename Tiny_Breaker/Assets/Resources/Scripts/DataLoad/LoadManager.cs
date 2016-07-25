using UnityEngine;
using System.Collections;
using Loader;

public class LoadManager : MonoBehaviour
{

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

    //[SerializeField]
    //GameObject playerCost;

    public IEnumerator Start()
    {
        WWW paramwww = new WWW(paramurl);
        WWW growwww = new WWW(growurl);
        WWW costwww = new WWW(costurl);

        yield return paramwww;
        yield return growwww;
        yield return costwww;

        string paramtext = paramwww.text;
        string growtext = growwww.text;
        string costtext = costwww.text;

        ParamData ParamTable = new ParamData();
        ParamTable.Load(paramwww.text);

        GrowData GrowTable = new GrowData();
        GrowTable.Load(growwww.text);

        CostData CostTable = new CostData();
        CostTable.Load(costwww.text);
        
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
                        SetGrow(grow, prePOPO);
                    break;
                case "pupu":
                    if (prePUPU != null)
                        SetGrow(grow, prePUPU);
                    break;
                case "pipi":
                    if (prePIPI != null)
                        SetGrow(grow, prePIPI);
                    break;
                default:
                    break;
            }
        }

        SetCost(CostTable);

        Debug.Log("Load END");
    }

    void SetParm(ParamMaster param, GameObject unit)
    {
        if (unit.GetComponent<Unit>())
            unit.GetComponent<Unit>().status.SetDefault(param.HP, param.ATK, param.SPEED, param.ATKSPEED);
    }

    void SetGrow(GrowMaster grow, GameObject unit)
    {
        if (unit.GetComponent<Demons>())
            unit.GetComponent<Demons>().GrowPoint.SetDefault(grow.GHP, grow.GATK, grow.GSPEED, grow.GATKSPEED);
    }

    void SetCost(CostData CostTable)
    {
        foreach (var cost in CostTable.All)
        {
            GameObject player = GameObject.Find("Player");
            if (player.GetComponent<PlayerCost>())
                player.GetComponent<PlayerCost>().SetDefault(cost.MaxCost, cost.StateCost, cost.CostParSecond, cost.DemonCost, cost.SoldierCost, cost.HouseCost);
        }
    }
}
