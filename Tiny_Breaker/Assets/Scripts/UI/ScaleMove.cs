using UnityEngine;
using System.Collections;

public class ScaleMove : MonoBehaviour
{
    [SerializeField, TooltipAttribute("拡大時間")]
    float expansionTime = 1.0f;
    [SerializeField, TooltipAttribute("拡大後の停止時間")]
    float expansionWaitTime = 1.0f;
    [SerializeField, TooltipAttribute("縮小時間")]
    float reductionTime = 1.0f;

    public IEnumerator ScaleUp(RectTransform rect)
    {
        bool end = false;
        float expansionCount = 0.0f;

        rect.localScale = Vector3.zero;

        while (!end)
        {
            if (expansionCount < expansionTime)
            {
                expansionCount += Time.deltaTime;
            }
            else
            {
                expansionCount = expansionTime;
                end = true;
            }

            float rate = expansionCount / expansionTime;

            rect.localScale = new Vector3(rate, rate, rate);

            yield return null;
        }

        yield return new WaitForSeconds(expansionWaitTime);
    }

    public IEnumerator ScaleDown(RectTransform rect)
    {
        bool end = false;
        float expansionCount = 0.0f;

        rect.localScale = Vector3.one;

        while (!end)
        {
            if (expansionCount < reductionTime)
            {
                expansionCount += Time.deltaTime;
            }
            else
            {
                expansionCount = reductionTime;
                end = true;
            }

            float rate = expansionCount / reductionTime;
            rate = 1.0f - rate;

            rect.localScale = new Vector3(rate, rate, rate);

            yield return null;
        }

        Destroy(this.transform.gameObject);
    }
}