//Unitに使える、
//Triggerの範囲内に入った対象に何かをする時の親クラス

using UnityEngine;

public class UnitTrigger : MonoBehaviour {

    //Unitクラスの親
    protected Unit parent;

    protected GameObject hitTarget;       //範囲内に入ったターゲット
    
    void OnTriggerEnter(Collider collider)
    {
        //悪魔が範囲内に入ってきたとき攻撃を開始する
        if (collider.gameObject == parent.targetObject)
        {
            if (!parent.IsDaed)
            {
                parent.IsAttack = true;
                hitTarget = collider.gameObject;
            }
            else
            {
                parent.IsAttack = false;
                hitTarget = null;
            }
        }
    }

    void OnTriggerStay(Collider collider)
    {
        //悪魔が範囲内に入っているとき攻撃を続ける
        if (collider.gameObject == parent.targetObject)
        {
            if (!parent.IsDaed)
            {
                parent.IsAttack = true;
                hitTarget = collider.gameObject;
            }
            else
            {
                parent.IsAttack = false;
                hitTarget = null;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        //範囲内から出たら攻撃をやめる
        if (collider.gameObject == parent.targetObject)
        {
            parent.IsAttack = false;
            hitTarget = null;
        }
    }
}
