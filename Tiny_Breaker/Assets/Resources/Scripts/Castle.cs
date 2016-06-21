using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Castle : MonoBehaviour
{
    //お城のの仮ステータス
    public int HP = 1000;
    public int HPpro { get { return HP; } set { HP = value; } }
    public int ATK = 100;
    public float AtackTime = 1.0f;

    private GameObject HPUI;
    private GameObject hitCollisionObject;

    void Start()
    {
        HPUI = GameObject.Find("CastleUI");
    }

    void Update()
    {
        HPUI.GetComponent<TextMesh>().text = "HP: " + HP.ToString();

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void DemonAtacked(GameObject target)
    {
        if(hitCollisionObject == target)
        {
            HP -= target.GetComponent<Demons>().ATKpro;
        }
    }

    //オブジェクトが触れている間
    void OnCollisionStay(Collision collision)
    {
        //接触しているゲームオブジェクトを保存
        hitCollisionObject = collision.gameObject;
    }
}
