using UnityEngine;
using System.Collections.Generic;

//大きい画面でのプレイヤークラス(スマホの情報の受け渡しとかプレイヤー番号とか送受信の解析とか)
public class Player : MonoBehaviour {

    //プレイヤーを識別する番号
    //誰の悪魔が魂の回収を行ったのかを判断するため必要だと思った
    public int playerID = 1;

    //画面になるプレファブ
    public GameObject screenObject;
    private GameObject instaceScreenObject;

    //スマホ側に送信するのに一時的にデータをためておく場所
    List<DemonsGrowPointData> spiritsData = new List<DemonsGrowPointData>();
    
	void Start ()
    {
        //プレイヤーの操作する画面のインスタンス化
        GameObject canvas = GameObject.Find("Canvas");
        instaceScreenObject = (GameObject)Instantiate(screenObject);
        instaceScreenObject.transform.SetParent(canvas.transform, false);
        instaceScreenObject.GetComponent<PlayerScreen>().PlayerID = playerID;
    }
	
	void Update () {

        //1フレーム同時に複数魂を回収することもあるかと思いこの形にした
        if (spiritsData.Count > 0)
        {
            foreach (var e in spiritsData)
            {
                //データをスマホ(になるであろう)画面に送る
                instaceScreenObject.GetComponent<PlayerScreen>().AddSpiritList(e);
            }

            //リストの送ったら中身を消す
            spiritsData.Clear();
        }
    }
    
    //魂リストへの追加
    public void AddSpiritList(DemonsGrowPointData spiritdata)
    {
        spiritsData.Add(spiritdata);
    }
}
