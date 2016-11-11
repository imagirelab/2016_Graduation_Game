using UnityEngine;
using UnityEngine.UI;

public class DebugCost : MonoBehaviour
{
    [SerializeField]
    Player player = null;
    [SerializeField]
    PlayerCost playerCost = null;

    int[] level = new int[(int)Enum.Demon_TYPE.Num];
    
    void Start()
    {
        level = player.DemonsLevel;
    }

    void Update()
    {
            this.GetComponent<Text>().text = playerCost.GetComponent<PlayerCost>().CurrentCost + "/" + playerCost.GetComponent<PlayerCost>().GetMaxCost + "\n" +
                                                "HouseCost:" + playerCost.GetHouseCost + "\n" +
                                                "SoldierCost:" + playerCost.GetSoldierCost + "\n" +
                                                "POPOCost:" +
                                                playerCost.GetCurrentDemonCost(level[(int)Enum.Demon_TYPE.POPO]) + "\n" +
                                                "PUPUCost:" +
                                                playerCost.GetCurrentDemonCost(level[(int)Enum.Demon_TYPE.PUPU]) + "\n" +
                                                "PIPICost:" +
                                                playerCost.GetCurrentDemonCost(level[(int)Enum.Demon_TYPE.PIPI]);
    }
}
