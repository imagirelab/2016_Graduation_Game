using UnityEngine;
using System.Collections;

public class StaticVariables : MonoBehaviour
{
    public static bool goFlag;
    public static Vector3 goPosition;

    //呼ばれたときに初期化
    void Awake()
    {
        goFlag = false;
        goPosition = Vector3.zero;
    }
}
