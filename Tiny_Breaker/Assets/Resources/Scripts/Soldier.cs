// 敵のステータスなどを管理するクラス

using UnityEngine;
using System.Collections;

public class Soldier : MonoBehaviour {

    //敵のステータス
    [SerializeField, TooltipAttribute("体力")]
    private int HP = 300;

    //このクラス内で使う変数
    private GameObject HP_UI;           //HPのUI

    //外から見れる変数
    public int HPpro { get { return HP; } set { HP = value; } }

    // Use this for initialization
    void Start () {

        HP_UI = transform.FindChild("HP").gameObject;
    }
	
	// Update is called once per frame
	void Update () {

        HP_UI.GetComponent<TextMesh>().text = "HP: " + HP.ToString();

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
