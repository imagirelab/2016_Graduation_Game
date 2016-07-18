//召喚ボタンを押したときの処理

using UnityEngine;
using UnityEngine.UI;

public class SummonButton : MonoBehaviour
{
    [SerializeField, TooltipAttribute("悪魔")]
    private GameObject demon;
    public GameObject Demon {
        get { return demon; }
        set { demon = value; }
    }
    
    [SerializeField, TooltipAttribute("出撃位置")]
    private Vector3 spawnPosition = Vector3.zero;
    public Vector3 SpawnPosition{ set { spawnPosition = value; } }
    
    void Start()
    {
        //仮に何も設定されていなかったら空のゲームオブジェクトを入れる
        if (demon == null)
            demon = new GameObject();
    }
    
    //クリックボタンが押された瞬間の処理
    public void OnClickDownSummon()
    {
        //指示画像を出す
        foreach (Transform child in transform)
            child.GetComponent<Image>().enabled = true;

        //半透明にしてみる
        gameObject.GetComponent<Image>().color = Vector4.one * 0.5f;
    }

    //クリックボタンが離されたときの処理
    public void OnClickUpSummon()
    {
        //指示画像を消す
        foreach (Transform child in transform)
            child.GetComponent<Image>().enabled = false;

        //色を元に戻す
        gameObject.GetComponent<Image>().color = Vector4.one;
    }

    //その場でのクリックが成立した時の処理
    public void OnClickSummon()
    {
        //悪魔を出す
        GameObject instaceObject = (GameObject)Instantiate(demon,
                                                           spawnPosition,           //プレイヤーごとの出撃位置
                                                           Quaternion.identity);
        GameObject playerObject = GameObject.Find("Player");        //別の方法でプレイヤーを取得方法を考えたい
        instaceObject.transform.SetParent(playerObject.transform, false);
        //悪魔に出すオーダークラスを渡す
        instaceObject.GetComponent<Demons>().Order = this.GetComponent<DemonsOrder>();
        instaceObject.GetComponent<Demons>().GrowPoint = demon.GetComponent<Demons>().GrowPoint;
    }
}
