using UnityEngine;
using StaticClass;

public class MultiDisplayScript : MonoBehaviour
{
    public int maxDisplayCount = 2;
    
    void Start()
    {
        for (int i = 0; i < maxDisplayCount && i < Display.displays.Length; i++)
        {
            if(!GameRule.getInstance().maltiOn)
                Display.displays[i].Activate();
        }
        GameRule.getInstance().maltiOn = true;
    }
}