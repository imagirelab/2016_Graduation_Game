using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    Image image;

    [SerializeField]
    float fadeInTime = 1.0f;
    [SerializeField]
    float fadeOutTime = 1.0f;
    float count = 0.0f;

    float alpha = 1.0f;
    
    void OnEnable()
    {
        image = GetComponent<Image>();
    }

    public void AlphaOffColor()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
    }

    public void AlphaOnColor()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
    }

    //フェードインのコルーチン
    public IEnumerator FadeInStart()
    {
        count = 0.0f;
        alpha = 1.0f;
        AlphaOnColor();

        while (true)
        {
            if (count < fadeInTime)
            {
                count += Time.deltaTime;
                float rate = count / fadeInTime;
                if (count >= fadeInTime)
                    rate = 1.0f;
                alpha = 1.0f * (1.0f - rate);

                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                yield return null;
            }
            else
            {
                AlphaOffColor();
                yield return null;
                break;
            }
        }
    }

    //フェードアウトのコルーチン
    public IEnumerator FadeOutStart()
    {
        count = 0.0f;
        alpha = 0.0f;
        AlphaOffColor();

        while (true)
        {
            if (count < fadeOutTime)
            {
                count += Time.deltaTime;
                float rate = count / fadeOutTime;
                if (count >= fadeOutTime)
                    rate = 1.0f;
                alpha = 1.0f * rate;

                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                yield return null;
            }
            else
            {
                AlphaOnColor();
                yield return null;
                break;
            }
        }
    }
}