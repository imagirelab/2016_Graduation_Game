using UnityEngine;
using StaticClass;

public class CameraMove : MonoBehaviour {
    
    [SerializeField, TooltipAttribute("カメラの離す距離")]
    Vector3 distance = new Vector3(0.0f, 70.0f, -70.0f);
    [SerializeField, TooltipAttribute("カメラが付き添う対象物")]
    GameObject target;

    Vector3 defaultPosition = Vector3.zero;

    void Start()
    {
        defaultPosition = transform.position;

        if (target == null)
            target = new GameObject();
    }
	
	void Update ()
    {
        Vector3 targetPosition = target.transform.position + distance;

        transform.position = targetPosition;
    }
}
