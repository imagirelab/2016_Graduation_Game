﻿using UnityEngine;
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
    GameObject[] player = new GameObject[GameRule.playerNum];
    [SerializeField]
    Spawner[] spawer;

    void Start ()
	{
        startCount = 0.0f;


    }
	
	void Update ()
	{
        //開始条件
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

        //終了条件
		if(P1Base.HPpro <= 0 || P2Base.HPpro <= 0 || time.End)
        {
            //残り体力判定（ラウンド結果判定）
            if(P1Base.HPpro > P2Base.HPpro)
            {
                GameRule.getInstance().round.Add(GameRule.ResultType.Player1Win);
            }
            if (P1Base.HPpro < P2Base.HPpro)
            {
                GameRule.getInstance().round.Add(GameRule.ResultType.Player2Win);
            }
            if (P1Base.HPpro == P2Base.HPpro)
            {
                GameRule.getInstance().round.Add(GameRule.ResultType.Draw);
            }

            //ラウンド数超えてるか判定
            if(GameRule.getInstance().round.Count >= GameRule.roundCount)
            {
                int p1wincount = 0;
                int p2wincount = 0;
                
                foreach(var e in GameRule.getInstance().round)
                {
                    if (e == GameRule.ResultType.Player1Win)
                        p1wincount++;
                    if (e == GameRule.ResultType.Player2Win)
                        p2wincount++;
                }

                //最終結果判定
                if (p1wincount > p2wincount)
                {
                    GameRule.getInstance().result = GameRule.ResultType.Player1Win;
                }
                if (p1wincount < p2wincount)
                {
                    GameRule.getInstance().result = GameRule.ResultType.Player2Win;
                }
                if (p1wincount == p2wincount)
                {
                    GameRule.getInstance().result = GameRule.ResultType.Draw;
                }

                //リザルトシーンへ
                SceneManager.LoadScene("ResultScene");
            }
            else
            {
                //ラウンド数が超えていなければもう一回
                SceneManager.LoadScene("MainScene");
            }
        }
	}
}