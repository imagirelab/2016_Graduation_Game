using UnityEngine;
using StaticClass;

public class ResultText : MonoBehaviour
{
    [SerializeField]
    Sprite[] resultUI = new Sprite[(int)Enum.ResultType.Num];

	void Start ()
	{
        SpriteRenderer image = GetComponent<SpriteRenderer>();

		switch(GameRule.getInstance().result)
        {
            case Enum.ResultType.Player1Win:
                image.sprite = resultUI[0];
                break;

            case Enum.ResultType.Player2Win:
                image.sprite = resultUI[1];
                break;

            case Enum.ResultType.Draw:
                image.sprite = resultUI[2];
                break;
                
            default:
                image.sprite = resultUI[2];
                break;
        }
	}
}