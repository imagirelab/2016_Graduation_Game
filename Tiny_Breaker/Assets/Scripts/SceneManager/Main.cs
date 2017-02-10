using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using StaticClass;
using SocketIO;

public class Main : MonoBehaviour
{
    #region フィールド

    //ラウンド中かどうかのフラグ
    public static bool roundEndFlag = false;

    [SerializeField]
    bool startFlag = true;  //スタート時の演出をするかしないかのフラグ

    [SerializeField]
    GameObject Pot1;
    DefenseBase P1Base;
    [SerializeField]
    GameObject Pot2;
    DefenseBase P2Base;

    //スタート準備物
    [SerializeField, HeaderAttribute("StartPreparation")]
    GameObject CueUI;

    [SerializeField]
    GameObject mainCamera;

    [SerializeField]
    LoadManager load;
    [SerializeField]
    GameObject fade;
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

    //ラウンド結果表示
    [SerializeField]
    GameObject roundResult;
    [SerializeField]
    GameObject roundBall;

    SocketIOComponent socket;

    #endregion

    void Start()
    {
        //フレームレートを固定
        Application.targetFrameRate = 30;

        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        if (Pot1 == null)
            Pot1 = new GameObject();
        if (Pot2 == null)
            Pot2 = new GameObject();

        P1Base = Pot1.GetComponent<DefenseBase>();
        P2Base = Pot2.GetComponent<DefenseBase>();

        if (CueUI == null)
            CueUI = new GameObject();
        CueUI.SetActive(false);
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
        //ちゃんとセットされていたら非アクティブにする
        if (roundResult == null)
            roundResult = new GameObject();
        else
            roundResult.SetActive(false);
        //ちゃんとセットされていたら非アクティブにする
        if (roundBall == null)
            roundBall = new GameObject();
        else
            roundBall.SetActive(false);

        //はじめのラウンドだけ
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

        socket.Emit("StopRequest");

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
            CueUI.SetActive(true);
            CueUI.GetComponent<Image>().sprite = CueUI.GetComponent<CueImage>().start;
            yield return StartCoroutine(CueUI.GetComponent<ScaleMove>().ScaleUp(CueUI.GetComponent<RectTransform>()));
            CueUI.SetActive(false);

        }

        //ゲームに使うものをオンにする
        SetGameMainActive(true);

        #endregion

        #region ゲーム中の処理

        socket.Emit("StopEndRequest");

        //終了条件
        bool finish = false;
        bool timeup = false;

        //ラウンド開始時は roundEndFlag を false
        roundEndFlag = false;

        while (true)
        {
            if (P1Base.HPpro <= 0 || P2Base.HPpro <= 0)
            {
                finish = true;
                break;
            }
            if (time.End)
            {
                timeup = true;
                break;
            }

            yield return null;
        }

        //ラウンド終了時は roundEndFlag を true
        roundEndFlag = true;

        yield return null;
        #endregion

        #region ゲーム終了後の処理

        socket.Emit("StopRequest");

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

        //ラウンド終了時に持っていたコストを保存しておく
        //if (GameRule.getInstance().round.Count < GameRule.roundCount)
        for (int i = 0; i < GameRule.playerNum; i++)
        {
            RoundDataBase.getInstance().PassesCost[i] = player[i].GetComponent<PlayerCost>().CurrentCost;

            RoundDataBase.getInstance().SetDemonLevel(player[i].tag,
                player[i].GetComponent<Player>().DemonsLevel[(int)Enum.Demon_TYPE.POPO],
                player[i].GetComponent<Player>().DemonsLevel[(int)Enum.Demon_TYPE.PUPU],
                player[i].GetComponent<Player>().DemonsLevel[(int)Enum.Demon_TYPE.PIPI]);
        }

        //ゲームに使うものをオフにする
        //プレイヤーが悪魔たちを消えるのを少し待つ
        foreach (var e in player)
            if (e.GetComponent<Player>())
                yield return StartCoroutine(e.GetComponent<Player>().ChildDead());

        SetGameMainActive(false);

        //終了演出

        //終了文字
        if(finish)
        {
            CueUI.SetActive(true);
            CueUI.GetComponent<Image>().sprite = CueUI.GetComponent<CueImage>().finish;
            yield return StartCoroutine(CueUI.GetComponent<ScaleMove>().ScaleUp(CueUI.GetComponent<RectTransform>()));
            CueUI.SetActive(false);
        }
        //タイムアップ文字
        if (timeup)
        {
            CueUI.SetActive(true);
            CueUI.GetComponent<Image>().sprite = CueUI.GetComponent<CueImage>().timeup;
            yield return StartCoroutine(CueUI.GetComponent<ScaleMove>().ScaleUp(CueUI.GetComponent<RectTransform>()));
            CueUI.SetActive(false);
        }

        roundResult.SetActive(true);
        //ラウンド結果表示が終わったら、
        RoundResultUI rou = roundResult.GetComponent<RoundResultUI>();
        ScaleMove sca = roundResult.GetComponent<ScaleMove>();
        yield return StartCoroutine(sca.ScaleUp(rou.RoundResult.GetComponent<RectTransform>()));
        StartCoroutine(sca.ScaleDown(rou.RoundResult.GetComponent<RectTransform>()));
        yield return null;

        if (GameRule.getInstance().round[GameRule.getInstance().round.Count - 1] != Enum.ResultType.Draw)
        {
            roundBall.SetActive(true);
            yield return StartCoroutine(roundBall.GetComponent<RoundBall>().BallMove());
            roundBall.SetActive(false);
        }

        //フェードを始める、
        yield return StartCoroutine(fade.GetComponent<Fade>().FadeOutStart());
        
        //シーン推移
        if (GameRule.getInstance().round.Count >= GameRule.roundCount ||
            p1wincount >= Mathf.CeilToInt(GameRule.roundCount / 2.0f) ||
            p2wincount >= Mathf.CeilToInt(GameRule.roundCount / 2.0f))
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
            socket.Emit("GameEndRequest");
            SceneManager.LoadScene("ResultScene");
        }
        else
        {
            //ラウンド数が超えていなければもう一回
            SceneManager.LoadScene("MainScene");
        }
        
        #endregion
    }

    void SetGameMainActive(bool value)
    {
        time.enabled = value;
        debugcontrol.SetActive(value);
        foreach (var e in player)
        {
            if (e.GetComponent<Player>())
                e.GetComponent<Player>().enabled = value;
            if (e.GetComponent<PlayerCost>())
                e.GetComponent<PlayerCost>().enabled = value;
        }
        foreach (var e in spawer)
            e.enabled = value;
        receiver.SetActive(value);
    }

    public void StopRequest()
    {
        socket.Emit("StopRequest");
    }

    public void StopEndRequest()
    {
        socket.Emit("StopEndRequest");
    }
}