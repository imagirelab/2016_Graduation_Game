using UnityEngine;
using StaticClass;

public class DebugHouse : MonoBehaviour
{
    House house;
    
    void Start()
    {
        if (transform.root.gameObject.GetComponent<House>())
            house = transform.root.gameObject.GetComponent<House>();
        else
            house = new House();
    }

    void Update ()
	{
        if (GameRule.getInstance().debugFlag)
        {
            if (GetComponent<TextMesh>() != null)
                GetComponent<TextMesh>().text = "HP:" + house.HPpro.ToString();
        }
    }
}