//UIをカメラの方に向かせるクラス

using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField]
    Camera camera;

    void Start()
    {
        if (camera == null)
            camera = Camera.main;
    }

    void Update()
    {
        transform.forward = camera.transform.forward;
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}
