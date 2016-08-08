using UnityEngine;
using StaticClass;

public class DebugStatusUI : MonoBehaviour
{
    [SerializeField]
    GameObject unit;
    private GameObject _MainCamera;

    void Start()
    {
        if (unit == null)
            unit = new GameObject(this.ToString() + " unit");

        _MainCamera = GameObject.Find("Main Camera");
    }

    void Update()
    {
        transform.LookAt(_MainCamera.transform);
        transform.forward = _MainCamera.transform.forward;

        //体力文字をデバッグフラグで表示非表示を切り替える
        if (GetComponent<MeshRenderer>() != null)
            GetComponent<MeshRenderer>().enabled = GameRule.getInstance().debugFlag;

        if (GameRule.getInstance().debugFlag)
        {
            if (GetComponent<TextMesh>() != null)
                GetComponent<TextMesh>().text = "HP:" + unit.GetComponent<Unit>().status.CurrentHP;
        }
    }
}