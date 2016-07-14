using UnityEngine;
using System.Collections;

public class SpiritCatcher : MonoBehaviour {

    //回収する魂の入れ物
    private GameObject clickSpirit;
    public GameObject GetClickSpirit { get { return clickSpirit; } }

    private int spiritCount = 0;

    // Use this for initialization
    void Start () {
        clickSpirit = null;
    }
	
	// Update is called once per frame
	void Update () {
        //条件
        if (this.GetComponent<PlayerControl>().CurrentOrder == PlayerControl.Order.Catcher)
        {
            //まだ回収していなければ登録する
            if (clickSpirit == null)
            {                
                //魂の移動場所(仮置きだから解像度とかで場所が変わるかも)
                clickSpirit = this.GetComponent<PlayerControl>().FieldCommand.GetComponent<MouseControl>().ClickGameObject;
                this.GetComponent<PlayerControl>().FieldCommand.GetComponent<MouseControl>().ClickGameObject.transform.position
                    = new Vector3(-17.0f, 29.0f, -32.0f);   //マジックナンバー
                spiritCount++;
            }
        }
    }
}
