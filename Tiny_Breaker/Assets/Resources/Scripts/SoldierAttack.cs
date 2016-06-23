using UnityEngine;
using System.Collections;

public class SoldierAttack : MonoBehaviour {

    public int ATK = 100;               //攻撃力
    public float AtackTime = 1.0f;      //攻撃間隔
    private float time;                 //時間

    bool IsAttack;                      //攻撃中フラグ
    GameObject AttackTarget;            //攻撃対象

    // Use this for initialization
    void Start () {
        IsAttack = false;

    }
	
	// Update is called once per frame
	void Update () {

        //アタックタイムを満たしていて攻撃フラグが立っていたら攻撃
        if (time > AtackTime && IsAttack)
        {
            time = 0;

            if (AttackTarget != null)
            {
                AttackTarget.GetComponent<Demons>().HPpro -= ATK;
                Debug.Log("Soldier Attack");
            }
            else
                IsAttack = false;
        }

        //1フレームあたりの時間を取得
        time += Time.deltaTime;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Unit" &&
            collider.gameObject.GetComponent<Demons>() != null &&
            AttackTarget == null)
        {
            IsAttack = true;
            AttackTarget = collider.gameObject;
            Debug.Log(collider.gameObject.name);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        //まだ触れているのに攻撃対象がいなかった場合別のターゲットが範囲内に入っていることになるので
        //そちらのほうに攻撃対象を移す
        if (collider.gameObject.tag == "Unit" &&
            collider.gameObject.GetComponent<Demons>() != null)
        {
            if (AttackTarget == null && collider.gameObject != null)
            {
                AttackTarget = collider.gameObject;
                Debug.Log(collider.gameObject.name);
            }
        }
        //悪魔以外が範囲内に入っていたら攻撃をやめる
        else
        {
            AttackTarget = null;
            IsAttack = false;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        AttackTarget = null;
        IsAttack = false;
    }
}
