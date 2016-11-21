using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using StaticClass;

public class Main : MonoBehaviour
{
    #region フィールド

    [SerializeField]
    bool startFlag = true;  //スタート時の演出をするかしないかのフラグ

    [SerializeField]
    DefenseBase P1Base;
    [SerializeField]
    DefenseBase P2Base;

    [SerializeField, HeaderAttribute("StartPreparation")]
    Image StartUI;

    [SerializeField]
    GameObject mainCamera;

    [SerializeField]
    LoadManager load;
    [SerializeField]
    GameObject fade;

    [SerializeField]
    float startEndTime = 1.0f;

    //スタート準備物
    [SerializeField]
    TimeLimit time;
    [SerializeField]
    GameObject debugcontrol;
    [SerializeField]
    GameObject[] player = new GameObject[GameRule.playerNum];
    [SerializeField]
    Spawner[] spawer;
    [SerializeField]
    GameObject receiver;

    #endregion

    void Start()
    {
        //フレームレートを固定
        Application.targetFrameRate = 30;

        if (P1Base == null)
            P1Base = new DefenseBase();
        if (P2Base == null)
            P2Base = new DefenseBase();
        if (StartUI == null)
            StartUI = null;
        StartUI.enabled = false;
        if (mainCamera == null)
            mainCamera = new GameObject();

        if (load == null)
            load = new LoadManager();
        if (fade == null)
            fade = new GameObject();

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
        if (receiver == null)
            receiver = new GameObject();

        if (GameRule.getInstance().round.Count == 0)
            mainCamera.GetComponent<CameraStartMove>().enabled = true;
        else
            mainCamera.GetComponent<CameraStartMove>().enabled = false;

        //スタート時に演出しないときの設定
        if (!startFlag)
        {
            fade.GetComponent<Fade>().AlphaOffColor();
            mainCamera.GetComponent<CameraStartMove>().enabled = false;
        }

        //コルーチンスタート
        StartCoroutine(MainUpdate());
    }

    IEnumerator MainUpdate()
    {
        #region ゲーム開始前の処理

        while (!load.LoadEnd)
            yield return null;

        //スタートの演出
        if (startFlag)
        {
            //フェードを始めて、
            yield return StartCoroutine(fade.GetComponent<Fade>().FadeInStart());

            //初めのカメラの動きを始めて、
            if (GameRule.getInstance().round.Count == 0)
                yield return StartCoroutine(mainCamera.GetComponent<CameraStartMove>().StartMove());

            //スタートを表示して、
            StartUI.enabled = true;

            yield return new WaitForSeconds(startEndTime);

            StartUI.enabled = false;
        }

        //ゲームを始めるものをオンにする
        time.enabled = true;
        debugcontrol.SetActive(true);
        foreach (var e in player)
            e.SetActive(true);
        foreach (var e in spawer)
            e.enabled = true;
        receiver.SetActive(true);

        #endregion

        #region ゲーム中の処理

        //終了条件
        while (!(P1Base.HPpro <= 0 || P2Base.HPpro <= 0 || time.End))
            yield return null;

        #endregion

        #region ゲーム終了後の処理

        //残り体力判定（ラウンド結果判定）
        if (P1Base.HPpro > P2Base.HPpro)
        {
            GameRule.getInstance().round.Add(Enum.ResultType.Player1Win);
        }
        if (P1Base.HPpro < P2Base.HPpro)
        {
            GameRule.getInstance().round.Add(Enum.ResultType.Player2Win);
        }
        if (P1Base.HPpro == P2Base.HPpro)
        {
            GameRule.getInstance().round.Add(Enum.ResultType.Draw);
        }

        //ラウンド数超えてるか判定
        int p1wincount = 0;
        int p2wincount = 0;

        foreach (var e in GameRule.getInstance().round)
        {
            if (e == Enum.ResultType.Player1Win)
                p1wincount++;
            if (e == Enum.ResultType.Player2Win)
                p2wincount++;
        }

        if (GameRule.getInstance().round.Count >= GameRule.roundCount)
        {
            //最終結果判定
            if (p1wincount > p2wincount)
            {
                GameRule.getInstance().result = Enum.ResultType.Player1Win;
            }
            if (p1wincount < p2wincount)
            {
                GameRule.getInstance().result = Enum.ResultType.Player2Win;
            }
            if (p1wincount == p2wincount)
            {
                GameRule.getInstance().result = Enum.ResultType.Draw;
            }

            //リザルトシーンへ
            SceneManager.LoadScene("ResultScene");
        }
        else
        {
            //ラウンド終了時に持っていたコストを保存しておく
            for (int i = 0; i < GameRule.playerNum; i++)
                RoundDataBase.getInstance().PassesCost[i] = player[i].GetComponent<PlayerCost>().CurrentCost;

            //ラウンド数が超えていなければもう一回
            SceneManager.LoadScene("MainScene");
        }

        #endregion
    }
}