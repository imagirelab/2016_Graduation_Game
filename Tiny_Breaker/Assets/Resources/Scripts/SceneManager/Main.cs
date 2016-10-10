using UnityEngine;
using UnityEngine.SceneManagement;
using StaticClass;

public class Main : MonoBehaviour
{
    [SerializeField]
    DefenseBase P1Base;
    [SerializeField]
    DefenseBase P2Base;

    [SerializeField]
    TimeLimit time;

    void Start ()
	{
		
	}
	
	void Update ()
	{
		if(P1Base.HPpro <= 0 || P2Base.HPpro <= 0)
        {
            if(P1Base.HPpro > 0 && P2Base.HPpro <= 0)
            {
                GameRule.getInstance().result = GameRule.ResultType.Player1Win;
            }
            if (P1Base.HPpro <= 0 && P2Base.HPpro > 0)
            {
                GameRule.getInstance().result = GameRule.ResultType.Player2Win;
            }
            if (P1Base.HPpro <= 0 && P2Base.HPpro <= 0)
            {
                GameRule.getInstance().result = GameRule.ResultType.Draw;
            }

            SceneManager.LoadScene("ResultScene");
        }

        if(time.End)
        {
            GameRule.getInstance().result = GameRule.ResultType.TimeUp;

            SceneManager.LoadScene("ResultScene");
        }
	}
}