//UIをカメラの方に向かせるクラス

using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour
{
    Coroutine cor;

    [SerializeField]
    Camera cam;
    
    IEnumerator LookCamera()
    {
        if (cam == null)
            cam = Camera.main;

        while (true)
        {
            transform.forward = cam.transform.forward;
            yield return null;
        }
    }
    
    void OnEnable()
    {
        cor = StartCoroutine(LookCamera());
    }

    void OnDisable()
    {
        StopCoroutine(cor);
        //gameObject.SetActive(false);
    }
}
