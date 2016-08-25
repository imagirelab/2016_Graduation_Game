//UIをカメラの方に向かせるクラス

using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    
    Camera _MainCamera;

    void Start ()
    {
        _MainCamera = Camera.main;
    }
	
	void Update ()
    {
        transform.forward = _MainCamera.transform.forward;
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}
