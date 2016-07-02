//お城のステータスなどを管理するクラス

using UnityEngine;

public class Castle : MonoBehaviour
{
    //お城のステータス
    [SerializeField, TooltipAttribute("体力")]
    private int HP = 1000;
    
    //このクラス内で使う変数
    private GameObject HP_UI;           //HPのUI

    //外から見れる変数
    public int HPpro { get { return HP; } set { HP = value; } }


    void Start()
    {
        HP_UI = transform.FindChild("HP").gameObject;
    }

    void Update()
    {
        HP_UI.GetComponent<TextMesh>().text = "HP: " + HP.ToString();

        if (HP <= 0)
        {
            Destroy(gameObject);
        }

    }
}
