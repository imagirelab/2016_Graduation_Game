using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    [SerializeField]
    float intervalTime = 1.0f;

    Image image;

    Coroutine cor;

    IEnumerator Flashing()
    {
        image = GetComponent<Image>();

        while (true)
        {
            image.color = new Color(image.color.r,
                                    image.color.g,
                                    image.color.b,
                                    1.0f);
            yield return new WaitForSeconds(intervalTime);

            image.color = new Color(image.color.r,
                                    image.color.g,
                                    image.color.b,
                                    0.0f);
            yield return new WaitForSeconds(intervalTime);
        }
    }

    void OnEnable()
    {
        cor = StartCoroutine(Flashing());
    }

    void OnDisable()
    {
        StopCoroutine(cor);
        image.color = new Color(image.color.r,
                                image.color.g,
                                image.color.b,
                                0.0f);
    }
}