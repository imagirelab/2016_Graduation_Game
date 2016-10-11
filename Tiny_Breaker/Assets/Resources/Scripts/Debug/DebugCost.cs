using UnityEngine;
using UnityEngine.UI;

public class DebugCost : MonoBehaviour
{

    [SerializeField]
    GameObject player;

    // Use this for initialization
    void Start()
    {
        if (player == null)
            player = new GameObject(this.ToString() + " nothing player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerCost>())
        {
            this.GetComponent<Text>().text = player.name.ToString() + ":" + player.GetComponent<PlayerCost>().CurrentCost + "/" + player.GetComponent<PlayerCost>().GetMaxCost + "\n" +
                                                "HouseCost:" + player.GetComponent<PlayerCost>().GetHouseCost + "\n" +
                                                "SoldierCost:" + player.GetComponent<PlayerCost>().GetSoldierCost + "\n" +
                                                "POPOCost:" +
                                                player.GetComponent<PlayerCost>().GetCurrentDemonCost(player.GetComponent<Player>().PlayerGrowPoint(GrowPoint.Type.POPO).Level) + "\n" +
                                                "PUPUCost:" +
                                                player.GetComponent<PlayerCost>().GetCurrentDemonCost(player.GetComponent<Player>().PlayerGrowPoint(GrowPoint.Type.PUPU).Level) + "\n" +
                                                "PIPICost:" +
                                                player.GetComponent<PlayerCost>().GetCurrentDemonCost(player.GetComponent<Player>().PlayerGrowPoint(GrowPoint.Type.PIPI).Level);
        }
    }
}
