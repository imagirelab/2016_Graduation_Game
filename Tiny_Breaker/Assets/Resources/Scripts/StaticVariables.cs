using UnityEngine;

public class StaticVariables : MonoBehaviour
{
    public static bool goFlag;
    public static Vector3 goPosition;
    public static bool catcherFlag;

    //呼ばれたときに初期化
    void Awake()
    {
        InitVariables();
    }

    public static void InitVariables()
    {
        goFlag = false;
        goPosition = Vector3.zero;
        catcherFlag = false;
    }
}
