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
    GameObject Pot1;
    DefenseBase P1Base;
    [SerializeField]
    GameObject Pot2;
    DefenseBase P2Base;

    //スタート準備物
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

    #endregion

    void Start()
    {
        //フレームレートを固定
        Application.targetFrameRate = 30;

        P1Base = Pot1.GetComponent<DefenseBase>();
        P2Base = Pot2.GetComponent<DefenseBase>();

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
        //ちゃんとセットされていたら非アクティブにする
        if (roundResult == null)
            roundResult = new GameObject();
        else
            roundResult.SetActive(false);
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

        //ゲームに使うものをオンにする
        SetGameMainActive(true);

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

        //ラウンド終了時に持っていたコストを保存しておく
        if (GameRule.getInstance().round.Count < GameRule.roundCount)
            for (int i = 0; i < GameRule.playerNum; i++)
                RoundDataBase.getInstance().PassesCost[i] = player[i].GetComponent<PlayerCost>().CurrentCost;

        //ゲームに使うものをオフにする
        //プレイヤーが悪魔たちを消えるのを少し待つ
        foreach (var e in player)
            if (e.GetComponent<Player>())
                yield return StartCoroutine(e.GetComponent<Player>().ChildDead());

        SetGameMainActive(false);

        //終了演出
        roundResult.SetActive(true);
        //ラウンド結果表示が終わったら、
        yield return StartCoroutine(roundResult.GetComponent<RoundResultUI>().ScaleUp());
        //しばらく表示
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(roundResult.GetComponent<RoundResultUI>().ScaleDown());
        yield return null;

        roundBall.SetActive(true);
        //ラウンドボールを動かす
        //ポットの上から出す方法
        //switch (GameRule.getInstance().round[GameRule.getInstance().round.Count - 1])
        //{
        //    case Enum.ResultType.Player1Win:
        //        roundBall.GetComponent<RoundBall>().StartPosition = Pot1.transform.position + new Vector3(0.0f, 20.0f, 0.0f);
        //        break;
        //    case Enum.ResultType.Player2Win:
        //        roundBall.GetComponent<RoundBall>().StartPosition = Pot2.transform.position + new Vector3(0.0f, 20.0f, 0.0f);
        //        break;
        //    default:
        //        break;
        //}
        yield return StartCoroutine(roundBall.GetComponent<RoundBall>().BallMove());
        roundBall.SetActive(false);

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
}