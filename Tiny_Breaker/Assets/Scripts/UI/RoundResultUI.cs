using UnityEngine;
using System.Collections;
using StaticClass;

public class RoundResultUI : MonoBehaviour
{
    [SerializeField]
    GameObject[] roundResults = new GameObject[(int)Enum.ResultType.Num];
    GameObject roundResult;

    [SerializeField]
    float expansionTime = 1.0f;
    [SerializeField]
    float reductionTime = 1.0f;
    float expansionCount = 0.0f;

    bool end = false;
    public bool End { get { return end; } }

    void OnEnable()
	{
        end = false;
        expansionCount = 0.0f;

        foreach (GameObject e in roundResults)
            e.SetActive(false);

        //一番後ろの値、最新の値
        switch (GameRule.getInstance().round[GameRule.getInstance().round.Count - 1])
        {
            case Enum.ResultType.Player1Win:
                roundResults[0].SetActive(true);
                roundResult = roundResults[0];
                break;
            case Enum.ResultType.Player2Win:
                roundResults[1].SetActive(true);
                roundResult = roundResults[1];
                break;
            case Enum.ResultType.Draw:
                roundResults[2].SetActive(true);
                roundResult = roundResults[2];
                break;
            default:
                break;
        }
    }

    public IEnumerator ScaleUp()
    {
        bool end = false;
        expansionCount = 0.0f;

        roundResult.GetComponent<RectTransform>().localScale = Vector3.zero;

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

            roundResult.GetComponent<RectTransform>().localScale = new Vector3(rate, rate, rate);

            yield return null;
        }
    }

    public IEnumerator ScaleDown()
    {
        bool end = false;
        expansionCount = 0.0f;

        roundResult.GetComponent<RectTransform>().localScale = Vector3.one;

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

            roundResult.GetComponent<RectTransform>().localScale = new Vector3(rate, rate, rate);

            yield return null;
        }

        Destroy(this.transform.gameObject);
    }
}