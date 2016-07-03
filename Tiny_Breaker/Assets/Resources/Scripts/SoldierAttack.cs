//敵の攻撃判定に関するクラス

using UnityEngine;
using System.Collections;

public class SoldierAttack : MonoBehaviour {

    //敵のステータス
    [SerializeField, TooltipAttribute("攻撃力")]
    private int ATK = 100;               //攻撃力
    [SerializeField, TooltipAttribute("攻撃間隔")]
    private float AtackTime = 1.0f;      //攻撃間隔

    //このクラス内で使う変数
    private float time;                 //時間
    private bool IsAttack;              //攻撃中フラグ
    private GameObject AttackTarget;    //攻撃対象
    

    void Start () {

        time = 0.0f;
        IsAttack = false;

    }
	
	void Update () {

        //アタックタイムを満たしていて攻撃フラグが立っていたら攻撃
        if (time > AtackTime && IsAttack)
        {
            time = 0;

            //攻撃対象がいることを確認してから攻撃
            if (AttackTarget != null)
                AttackTarget.GetComponent<Demons>().status.CurrentHP -= ATK;
            else
                IsAttack = false;
        }

        //1フレームあたりの時間を取得
        time += Time.deltaTime;
    }

    void OnTriggerEnter(Collider collider)
    {
        //悪魔が範囲内に入ってきたとき攻撃対象が登録されていなければ登録する
        if (collider.gameObject.GetComponent<Demons>() != null &&
            AttackTarget == null)
        {
            IsAttack = true;
            AttackTarget = collider.gameObject;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        //まだ触れているのに攻撃対象がいなかった場合別のターゲットが範囲内に入っていることになるので
        //そちらのほうに攻撃対象を移す
        if (collider.gameObject.GetComponent<Demons>() != null)
        {
            if (AttackTarget == null && collider.gameObject != null)
            {
                IsAttack = true;
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
        //範囲内から出たら攻撃対象から外す
        if (AttackTarget == collider.gameObject)
        {
            AttackTarget = null;
            IsAttack = false;
        }
    }
}
