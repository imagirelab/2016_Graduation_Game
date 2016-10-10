using UnityEngine;
using UnityEngine.UI;
using StaticClass;

public class ResultText : MonoBehaviour
{
	void Start ()
	{
		switch(GameRule.getInstance().result)
        {
            case GameRule.ResultType.Player1Win:
                GetComponent<Text>().text = "Player1 Win";
                GetComponent<Text>().color = Color.red;
                break;

            case GameRule.ResultType.Player2Win:
                GetComponent<Text>().text = "Player2 Win";
                GetComponent<Text>().color = Color.blue;
                break;

            case GameRule.ResultType.Draw:
                GetComponent<Text>().text = "Draw";
                GetComponent<Text>().color = Color.green;
                break;

            case GameRule.ResultType.TimeUp:
                GetComponent<Text>().text = "Time Up";
                GetComponent<Text>().color = Color.yellow;
                break;

            default:
                GetComponent<Text>().text = "None";
                GetComponent<Text>().color = Color.white;
                break;
        }
	}
	
	void Update ()
	{
		
	}
}