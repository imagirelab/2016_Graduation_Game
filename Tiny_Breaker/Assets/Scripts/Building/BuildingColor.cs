using UnityEngine;

public class BuildingColor : MonoBehaviour
{
    [SerializeField]
    Material[] mat = new Material[3];
    
    GameObject house;

    void Start()
    {
        house = transform.parent.parent.parent.gameObject;
    }

    void Update()
    {
        if (GetComponent<MeshRenderer>() != null)
            switch (house.tag)
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
