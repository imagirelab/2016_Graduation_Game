using UnityEngine;
using StaticClass;

public class DebugMeshSwitch : MonoBehaviour
{
	void Update ()
	{
        //体力文字をデバッグフラグで表示非表示を切り替える
        if (GetComponent<MeshRenderer>() != null)
            GetComponent<MeshRenderer>().enabled = GameRule.getInstance().debugFlag;
    }
}