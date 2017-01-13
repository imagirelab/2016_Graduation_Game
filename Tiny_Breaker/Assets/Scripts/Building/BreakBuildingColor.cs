using UnityEngine;
using System.Collections;

public class BreakBuildingColor : MonoBehaviour
{
    [SerializeField]
    Material[] mat = new Material[3];
    
    House house;

    void Start()
    {
        house = transform.parent.parent.parent.parent.gameObject.GetComponent<House>();

        if (GetComponent<MeshRenderer>() != null && house)
            switch (house.OldTag)
            {
                case "Player1":
                    GetComponent<MeshRenderer>().material = mat[0];
                    break;
                case "Player2":
                    GetComponent<MeshRenderer>().material = mat[1];
                    break;
                default:
                    GetComponent<MeshRenderer>().material = mat[2];
                    break;
            }
    }
}
