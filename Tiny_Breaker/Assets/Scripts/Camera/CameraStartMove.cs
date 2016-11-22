using UnityEngine;
using System.Collections;

public class CameraStartMove : MonoBehaviour
{
    [SerializeField]
    Vector3 startPosition = Vector3.zero;
    [SerializeField]
    Vector3 goalPosition = Vector3.zero;
    Vector3 moveVec = Vector3.zero;

    [SerializeField]
    float waitTime = 1.0f;
    [SerializeField]
    float moveTime = 1.0f;
    float time = 0.0f;

    void OnEnable()
    {
        transform.position = startPosition;
        moveVec = goalPosition - startPosition;
        time = 0.0f;
    }

    void OnDisable()
    {
        transform.position = goalPosition;
    }

    public IEnumerator StartMove ()
	{
        yield return new WaitForSeconds(waitTime);

        while (true)
        {
            time += Time.deltaTime;
            
            if (time >= moveTime)
                time = moveTime;

            float rate = time / moveTime;

            transform.position = startPosition + (moveVec * rate);

            yield return null;

            if (time >= moveTime)
                    break;
        }
    }
}