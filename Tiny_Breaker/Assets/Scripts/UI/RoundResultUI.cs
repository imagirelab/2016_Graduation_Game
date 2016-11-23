using UnityEngine;
using StaticClass;

public class RoundResultUI : MonoBehaviour
{
    [SerializeField]
    GameObject[] roundResults = new GameObject[(int)Enum.ResultType.Num];
    GameObject roundResult;
    public GameObject RoundResult { get { return roundResult; } }
    
    void OnEnable()
	{
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
}