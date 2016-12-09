using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using StaticClass;
using SocketIO;

public class Title : MonoBehaviour
{
    //フェードさせるオブジェクト
    [SerializeField]
    GameObject fade;

    [SerializeField]
    GameObject[] ok = new GameObject[GameRule.playerNum];

    SocketIOComponent socket;

    void Start()
    {
        //静的なデータの初期化
        GameRule.getInstance().Reset();
        RoundDataBase.getInstance().Reset();

        //オブジェクトの設定し忘れ
        if (fade == null)
            fade = new GameObject();
        
        //コルーチンスタート
        StartCoroutine(TitleUpdate());
    }

    IEnumerator TitleUpdate()
    {
        //フェードイン終了
        yield return StartCoroutine(fade.GetComponent<Fade>().FadeInStart());
        
        while (true)
        {
            //ボタンを押されたらフェードアウト開始
            if (Input.GetKeyDown(KeyCode.Return))
            {
                //フェードイン終了
                yield return StartCoroutine(fade.GetComponent<Fade>().FadeOutStart());

                break;
            }

            //プレイヤーがログインしたらフェード
            if (ok[0].activeInHierarchy && ok[1].activeInHierarchy)
            {
                //フェードイン終了
                yield return StartCoroutine(fade.GetComponent<Fade>().FadeOutStart());

                break;
            }

            yield return null;
        }
        
        //終了リクエスト
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.Emit("StopRequest");

        //フェードアウト終了時シーン切り替え
        SceneManager.LoadScene("MainScene");

        yield return null;
    }
}