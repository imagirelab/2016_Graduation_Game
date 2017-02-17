using UnityEngine;
using System.Collections;

public class SubCamera : MonoBehaviour
{
    public float changeTime = 0.0f;

    public Vector3[] positions;
    public Vector3[] rotation;

    void Start ()
	{
        StartCoroutine(ChangePosition());
	}

    IEnumerator ChangePosition()
    {
        int count = 0;
        while (true)
        {
            if (count >= positions.Length)
                count = 0;

            transform.position = positions[count];
            transform.rotation = Quaternion.Euler(rotation[count]);

            count++;

            yield return new WaitForSeconds(changeTime);
        }
    }
}