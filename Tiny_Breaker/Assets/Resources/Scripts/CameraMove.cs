using UnityEngine;
using StaticClass;

public class CameraMove : MonoBehaviour {
    
    [SerializeField, TooltipAttribute("カメラの離す距離")]
    Vector3 distance = new Vector3(0.0f, 70.0f, -70.0f);
    [SerializeField, TooltipAttribute("カメラが付き添う対象物")]
    GameObject target;

    [SerializeField]
    Vector3 allFieldPosition = new Vector3(0.0f, 180.0f, -210.0f);
    bool IsAllSeeing = false;

    void Start()
    {
        if (target == null)
            target = new GameObject();

        IsAllSeeing = false;
    }
	
	void Update ()
    {
        transform.position = target.transform.position + distance;

        //デバッグ表示の時のカメラ
        if (GameRule.getInstance().debugFlag)
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
                IsAllSeeing = !IsAllSeeing;

            //ステージ全体表示
            if(IsAllSeeing)
                transform.position = allFieldPosition;
        }
    }
}
