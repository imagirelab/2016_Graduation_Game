//攻撃範囲に関するクラス

using UnityEngine;

public class AttackRange : MonoBehaviour
{
    //Unitクラスの親
    Unit parent;

    //このクラス内で使う変数
    float time;                 //時間
    GameObject hitTarget;       //範囲内に入ったターゲット

    void Start () {

        //親にUnitClassを継承しているスクリプトを持っていたら登録する
        if (transform.parent.GetComponent<Unit>())
            parent = transform.parent.GetComponent<Unit>();
        else
            Debug.Log("AttackRange: parent =" + parent);
        
        time = 0.0f;
        parent.IsAttack = false;

    }
	
	void Update () {
        
        //アタックタイムを満たしていて攻撃フラグが立っていたら攻撃
        if (time > parent.status.CurrentAtackTime && parent.IsAttack)
        {
            time = 0;

            //攻撃対象がいることを確認してから攻撃
            if (hitTarget != null)
            {
                if (parent.attackTarget.GetComponent<Unit>())
                    parent.attackTarget.GetComponent<Unit>().status.CurrentHP -= parent.status.CurrentATK;
                //建物への攻撃はこっち
            }
            else
                parent.IsAttack = false;
        }

        //1フレームあたりの時間を取得
        time += Time.deltaTime;
    }

    void OnTriggerEnter(Collider collider)
    {
        //悪魔が範囲内に入ってきたとき攻撃を開始する
        if (collider.gameObject == parent.attackTarget)
        {
            parent.IsAttack = true;
            hitTarget = collider.gameObject;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        //悪魔が範囲内に入っているとき攻撃を続ける
        if (collider.gameObject == parent.attackTarget)
        {
            parent.IsAttack = true;
            hitTarget = collider.gameObject;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        //範囲内から出たら攻撃をやめる
        if (collider.gameObject == parent.attackTarget)
        {
            parent.IsAttack = false;
            hitTarget = null;
        }
    }
}
