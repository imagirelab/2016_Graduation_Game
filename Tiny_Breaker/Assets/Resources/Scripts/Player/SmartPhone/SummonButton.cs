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
        //今だけコスト引くために設定する形
        GameObject playerCost = GameObject.Find("Player"); 

        //コストを確認して召喚する処理
        if (playerCost.GetComponent<PlayerCost>().UseableCost(playerCost.GetComponent<PlayerCost>().GetDemonCost * demon.GetComponent<Demons>().GrowPoint.GetCost()))
        {
            //適当な値を入れて重なることを避ける
            Vector3 randVac = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            //悪魔を出す
            GameObject instaceObject = (GameObject)Instantiate(demon,
                                                               spawnPosition + demon.transform.position + randVac,           //プレイヤーごとの出撃位置
                                                               Quaternion.identity);
            GameObject playerObject = GameObject.Find("Player");        //別の方法でプレイヤーを取得方法を考えたい
            instaceObject.transform.SetParent(playerObject.transform, false);
            //悪魔に出すオーダークラスを渡す
            instaceObject.GetComponent<Demons>().Order = this.GetComponent<DemonsOrder>();
            instaceObject.GetComponent<Demons>().GrowPoint = demon.GetComponent<Demons>().GrowPoint;
        }
    }
}
