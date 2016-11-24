using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using StaticClass;

public class Title : MonoBehaviour
{
    //フェードさせるオブジェクト
    [SerializeField]
    GameObject fade;

    //フラッシュさせるオブジェクト
    [SerializeField]
    GameObject flash;

    void Start()
    {
        //静的なデータの初期化
        GameRule.getInstance().Reset();
        RoundDataBase.getInstance().Reset();

        //オブジェクトの設定し忘れ
        if (fade == null)
            fade = new GameObject();

        if (flash == null)
            flash = new GameObject();

        //コルーチンスタート
        StartCoroutine(TitleUpdate());
    }

    IEnumerator TitleUpdate()
    {
        //フェードイン終了
        yield return StartCoroutine(fade.GetComponent<Fade>().FadeInStart());

        //フラッシュを開始する
        if (flash.GetComponent<Flash>())
            if (!flash.GetComponent<Flash>().enabled)
                flash.GetComponent<Flash>().enabled = true;

        while (true)
        {
            //ボタンを押されたらフェードアウト開始
            if (Input.GetKeyDown(KeyCode.Return))
            {
                //フラッシュコンポーネントをオフにする
                if (flash.GetComponent<Flash>())
                    if (flash.GetComponent<Flash>().enabled)
                        flash.GetComponent<Flash>().enabled = false;

                //フェードイン終了
                yield return StartCoroutine(fade.GetComponent<Fade>().FadeOutStart());

                break;
            }

            yield return null;
        }

        //フェードアウト終了時シーン切り替え
        SceneManager.LoadScene("MainScene");

        yield return null;
    }
}