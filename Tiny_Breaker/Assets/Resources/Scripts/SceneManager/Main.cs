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
    [SerializeField]
    GameObject debugcontrol;
    [SerializeField]
    GameObject[] player = new GameObject[GameRule.playerNum];
    [SerializeField]
    Spawner[] spawer;

    void Start()
    {
        startCount = 0.0f;

        if (P1Base == null)
            P1Base = new DefenseBase();
        if (P2Base == null)
            P2Base = new DefenseBase();
        if (StartUI == null)
            StartUI = null;
        if (time == null)
            time = new TimeLimit();
        if (debugcontrol == null)
            debugcontrol = new GameObject();
        for (int i = 0; i < player.Length; i++)
            if (player[i] == null)
                player[i] = new GameObject();
        for (int i = 0; i < spawer.Length; i++)
            if (spawer[i] == null)
                spawer[i] = new Spawner();
        if (spawer.Length == 0)
        {
            spawer = new Spawner[1];
            spawer[0] = new Spawner();
        }
    }

    void Update()
    {
        //開始条件
        if (startCount >= startEndTime)
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
        if (P1Base.HPpro <= 0 || P2Base.HPpro <= 0 || time.End)
        {
            //残り体力判定（ラウンド結果判定）
            if (P1Base.HPpro > P2Base.HPpro)
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
            int p1wincount = 0;
            int p2wincount = 0;

            foreach (var e in GameRule.getInstance().round)
            {
                if (e == GameRule.ResultType.Player1Win)
                    p1wincount++;
                if (e == GameRule.ResultType.Player2Win)
                    p2wincount++;
            }

            if (p1wincount > ((float)GameRule.roundCount / 2.0f) ||
                p2wincount > ((float)GameRule.roundCount / 2.0f))
            {
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