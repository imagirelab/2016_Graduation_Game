using UnityEngine;
using System.Collections.Generic;
using StaticClass;

public class DebugControl : MonoBehaviour
{
    [SerializeField, TooltipAttribute("デバック表示するかどうか")]
    bool IsDebug = false;
    
    [SerializeField]
    List<GameObject> DebugObject = new List<GameObject>();

    void Start()
    {
        GameRule.getInstance().debugFlag = IsDebug;
        
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
    }

    void SetDebugActive(bool flag)
    {
        foreach(GameObject e in DebugObject)
            e.SetActive(flag);
    }
}
