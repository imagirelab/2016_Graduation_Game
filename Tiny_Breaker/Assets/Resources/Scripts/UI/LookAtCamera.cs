//UIをカメラの方に向かせるクラス

using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField]
    Camera cam;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        transform.forward = cam.transform.forward;
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}
