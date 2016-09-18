using UnityEngine;
using StaticClass;

public class StatusUI : MonoBehaviour
{
    void Update()
    {
        //デバッグによってオンオフ切り替え
        foreach (Transform child in transform)
            child.gameObject.SetActive(!GameRule.getInstance().debugFlag);
    }
}
