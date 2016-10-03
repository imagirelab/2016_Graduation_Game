using UnityEngine;
using StaticClass;

public class DebugHouse : MonoBehaviour
{
    House house;

    void Start()
    {
        if (transform.parent.gameObject.GetComponent<House>())
            house = transform.parent.gameObject.GetComponent<House>();
        else
            house = new House();
    }

    void Update()
    {
        if (GameRule.getInstance().debugFlag)
        {
            if (GetComponent<TextMesh>() != null)
                GetComponent<TextMesh>().text = "HP:" + house.HPpro.ToString();
        }
    }
}