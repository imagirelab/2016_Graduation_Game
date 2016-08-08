using UnityEngine;
using StaticClass;

public class DebugControl : MonoBehaviour
{
    [SerializeField]
    GameObject DebugCost;
    [SerializeField]
    GameObject DebugUnit;

    void Start()
    {
        GameRule.getInstance().debugFlag = true;

        if (DebugCost == null)
            DebugCost = new GameObject(this.ToString() + "DebugCost");
        if (DebugUnit == null)
            DebugUnit = new GameObject(this.ToString() + "DebugUnit");

        SetDebugActive(GameRule.getInstance().debugFlag);
    }

    void Update()
    {
        //デバッグ表示の切り替え
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            GameRule.getInstance().debugFlag = !GameRule.getInstance().debugFlag;

            SetDebugActive(GameRule.getInstance().debugFlag);
        }

        if (Input.GetKeyDown(KeyCode.Keypad9))
            DebugUnit.GetComponent<DebugUnit>().AddDemonsPage(1);
        if (Input.GetKeyDown(KeyCode.Keypad7))
            DebugUnit.GetComponent<DebugUnit>().AddDemonsPage(-1);
        if (Input.GetKeyDown(KeyCode.Keypad6))
            DebugUnit.GetComponent<DebugUnit>().AddSoldiersPage(1);
        if (Input.GetKeyDown(KeyCode.Keypad4))
            DebugUnit.GetComponent<DebugUnit>().AddSoldiersPage(-1);
    }

    void SetDebugActive(bool flag)
    {
        DebugCost.SetActive(flag);
        DebugUnit.SetActive(flag);
    }
}
