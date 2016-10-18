using UnityEngine;

public class BuildingColor : MonoBehaviour
{
    [SerializeField]
    Material[] mat = new Material[3];

    GameObject root;

    void Start()
    {
        root = transform.root.gameObject;
    }

    void Update()
    {
        if(GetComponent<MeshRenderer>() != null)
            switch(root.tag)
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
