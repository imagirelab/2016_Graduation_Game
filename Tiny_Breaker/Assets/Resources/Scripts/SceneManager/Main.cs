using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using StaticClass;

public class Main : MonoBehaviour
{
    [SerializeField]
    DefenseBase P1Base;
    [SerializeField]
    DefenseBase P2Base;
    
    [SerializeField, HeaderAttribute("StartPreparation")]
    Image StartUI;

    [SerializeField]
    float startEndTime = 1.0f;
    float startCount;

    //スタート準備物
    [SerializeField]
    TimeLimit time;
    //[SerializeField]
    //GameObject bgm;
    [SerializeField]
    GameObject debugcontrol;
    [SerializeField]
    GameObject[] player = new GameObject[GameRule.getInstance().playerNum];
    [SerializeField]
    Spawner[] spawer;

    void Start ()
	{
        startCount = 0.0f;
    }
	
	void Update ()
	{
        if(startCount >= startEndTime)
        {
            StartUI.enabled = false;

            //ゲームを始めるものをオンにする
            time.enabled = true;
            debugcontrol.SetActive(true);
            foreach (var e in player)
                e.SetActive(true);
            foreach (var e in spawer)
                e.enabled = true;
        }
        else
        {
            StartUI.enabled = true;
            startCount += Time.deltaTime;
        }

		if(P1Base.HPpro <= 0 || P2Base.HPpro <= 0 || time.End)
        {
            if(P1Base.HPpro > P2Base.HPpro)
            {
                GameRule.getInstance().result = GameRule.ResultType.Player1Win;
            }
            if (P1Base.HPpro < P2Base.HPpro)
            {
                GameRule.getInstance().result = GameRule.ResultType.Player2Win;
            }
            if (P1Base.HPpro == P2Base.HPpro)
            {
                GameRule.getInstance().result = GameRule.ResultType.Draw;
            }

            SceneManager.LoadScene("ResultScene");
        }
	}
}