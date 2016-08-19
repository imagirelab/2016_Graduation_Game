using UnityEngine;
using UnityEngine.UI;

public class DebugCost : MonoBehaviour
{

    [SerializeField]
    GameObject playerCost;
    [SerializeField]
    GameObject POPO;
    [SerializeField]
    GameObject PUPU;
    [SerializeField]
    GameObject PIPI;

    // Use this for initialization
    void Start()
    {
        if (playerCost == null)
            playerCost = new GameObject(this.ToString() + "playerCost");
        if (POPO == null)
            POPO = new GameObject(this.ToString() + "POPO");
        if (PUPU == null)
            PUPU = new GameObject(this.ToString() + "PUPU");
        if (PIPI == null)
            PIPI = new GameObject(this.ToString() + "PIPI");
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Text>().text = "Cost:" + playerCost.GetComponent<PlayerCost>().CurrentCost + "/" + playerCost.GetComponent<PlayerCost>().GetMaxCost + "\n" +
                                            "HouseCost:" + playerCost.GetComponent<PlayerCost>().GetHouseCost + "\n" +
                                            "SoldierCost:" + playerCost.GetComponent<PlayerCost>().GetSoldierCost + "\n" +
                                            "POPOCost:" + playerCost.GetComponent<PlayerCost>().GetDemonCost * POPO.GetComponent<Demons>().GrowPoint.GetCost() + "\n" +
                                            "PUPUCost:" + playerCost.GetComponent<PlayerCost>().GetDemonCost * PUPU.GetComponent<Demons>().GrowPoint.GetCost() + "\n" +
                                            "PIPICost:" + playerCost.GetComponent<PlayerCost>().GetDemonCost * PIPI.GetComponent<Demons>().GrowPoint.GetCost();
    }
}
