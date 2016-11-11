using UnityEngine;
using UnityEngine.UI;

public class SpiritColor : MonoBehaviour
{

    [SerializeField]
    Player player = null;

    Color spiritColor = Color.white;

    void Start()
    {
        spiritColor = Color.white;
    }

    void Update()
    {
        Enum.Demon_TYPE firstSpirit = player.GetFirstSpirit();

        switch (firstSpirit)
        {
            case Enum.Demon_TYPE.POPO:
                spiritColor = Color.blue;
                break;
            case Enum.Demon_TYPE.PUPU:
                spiritColor = Color.red;
                break;
            case Enum.Demon_TYPE.PIPI:
                spiritColor = Color.green;
                break;
            default:
                spiritColor = Color.white;
                break;
        }

        GetComponent<Image>().color = spiritColor;
    }
}
