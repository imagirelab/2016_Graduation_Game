using UnityEngine;
using StaticClass;

public class DebugDefenseBase : MonoBehaviour
{
    DefenseBase defenseBase;

    void Start()
    {
        if (transform.parent.gameObject.GetComponent<DefenseBase>())
            defenseBase = transform.parent.gameObject.GetComponent<DefenseBase>();
        else
            defenseBase = new DefenseBase();
    }

    void Update()
    {
        if (GameRule.getInstance().debugFlag)
        {
            if (GetComponent<TextMesh>() != null)
                GetComponent<TextMesh>().text = "HP:" + defenseBase.HPpro.ToString();
        }
    }
}