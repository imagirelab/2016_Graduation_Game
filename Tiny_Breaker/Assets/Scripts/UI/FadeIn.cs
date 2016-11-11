using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField]
    float fadeTime = 1.0f;
    float count = 0.0f;

    float alpha = 1.0f;

    bool end = false;
    public bool End { get { return end; } }

    void Start ()
	{
        alpha = 1.0f;
        count = 0.0f;
        GetComponent<Image>().color = new Color(GetComponent<Image>().color.r,
                                                GetComponent<Image>().color.g,
                                                GetComponent<Image>().color.b,
                                                alpha);
        end = false;
    }
	
	void Update ()
    {
        if (count < fadeTime)
        {
            count += Time.deltaTime;
            float rate = count / fadeTime;
            alpha = 1.0f * (1.0f - rate);
        }

        if (count >= fadeTime)
        {
            alpha = 0.0f;
            end = true;
        }

        GetComponent<Image>().color = new Color(GetComponent<Image>().color.r,
                                                GetComponent<Image>().color.g,
                                                GetComponent<Image>().color.b,
                                                alpha);
    }
}