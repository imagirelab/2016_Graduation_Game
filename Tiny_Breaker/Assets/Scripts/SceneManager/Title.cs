using UnityEngine;
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

    //シーン終了フラグ
    bool end = false;

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

        //シーン終了フラグの初期化
        end = false;

        //フェードイン開始
        if (fade.GetComponent<FadeIn>())
            fade.GetComponent<FadeIn>().enabled = true;
    }

    void Update()
    {
        //フェードイン終了
        if (fade.GetComponent<FadeIn>())
            if (fade.GetComponent<FadeIn>().enabled && fade.GetComponent<FadeIn>().End)
            {
                //フェードインコンポーネントをオフにする
                fade.GetComponent<FadeIn>().enabled = false;

                //フラッシュを開始する
                if (flash.GetComponent<Flash>())
                    if (!flash.GetComponent<Flash>().enabled)
                        flash.GetComponent<Flash>().enabled = true;
            }

        //ボタンを押されたらフェードアウト開始
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //シーン終了フラグを立てる
            end = true;

            //仮にまだフェードインコンポーネントがあった場合はオフにする
            if (fade.GetComponent<FadeIn>())
                if (fade.GetComponent<FadeIn>().enabled)
                    fade.GetComponent<FadeIn>().enabled = false;

            //フェードアウトコンポーネントを立てる
            if (fade.GetComponent<FadeOut>())
                if (!fade.GetComponent<FadeOut>().enabled)
                    fade.GetComponent<FadeOut>().enabled = true;

            //フラッシュコンポーネントをオフにする
            if (flash.GetComponent<Flash>())
                if (flash.GetComponent<Flash>().enabled)
                    flash.GetComponent<Flash>().enabled = false;
        }

        //フェードアウト終了時シーン切り替え
        if (end && !fade.GetComponent<FadeOut>())           //フェードアウトコンポーネントが設定されていなかった場合
            SceneManager.LoadScene("MainScene");
        else if (end && fade.GetComponent<FadeOut>().End)   //フェードアウトコンポーネントが設定されている場合
            SceneManager.LoadScene("MainScene");
    }
}