using UnityEngine;
using UnityEngine.UI;

public class SpiritColor : MonoBehaviour {

    [SerializeField]
    GameObject player;

    Color spiritColor = Color.white;

    void Start()
    {
        spiritColor = Color.white;

        if (player == null)
            player = new GameObject();
    }
	
	void Update () {
        GrowPoint firstSpirit = player.GetComponent<Player>().GetFirstSpirit();

        if (firstSpirit != null)
        {
            switch (firstSpirit.GetDemonType)
            {
                case GrowPoint.Type.POPO:
                    spiritColor = Color.blue;
                    break;
                case GrowPoint.Type.PUPU:
                    spiritColor = Color.red;
                    break;
                case GrowPoint.Type.PIPI:
                    spiritColor = Color.green;
                    break;
            }
        }
        else
            spiritColor = Color.white;

        GetComponent<Image>().color = spiritColor;
    }
}
