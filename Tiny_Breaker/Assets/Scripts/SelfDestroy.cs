using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour
{
    Coroutine cor;

    [SerializeField]
    float deadTime = 1.0f;

    IEnumerator SelfDead()
    {
        yield return new WaitForSeconds(deadTime);

        Destroy(gameObject);
    }

    void OnEnable()
    {
        cor = StartCoroutine(SelfDead());
    }

    void OnDisable()
    {
        StopCoroutine(cor);
    }
}