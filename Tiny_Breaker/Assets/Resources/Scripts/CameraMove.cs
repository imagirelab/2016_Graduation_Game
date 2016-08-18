using UnityEngine;

public class CameraMove : MonoBehaviour {
    
    [SerializeField, TooltipAttribute("カメラの離す距離")]
    Vector3 distance = new Vector3(0.0f, 70.0f, -70.0f);
    [SerializeField, TooltipAttribute("カメラが付き添う対象物")]
    GameObject target;

    void Start()
    {
        if (target == null)
            target = new GameObject();
    }
	
	void Update ()
    {
        Vector3 targetPosition = target.transform.position + distance;

        transform.position = targetPosition;
    }
}
