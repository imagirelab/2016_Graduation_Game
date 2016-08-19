//Unitに使える、
//Triggerの範囲内に入った対象に何かをする時の親クラス

using UnityEngine;

public class UnitTrigger : MonoBehaviour {

    //Unitクラスの親
    protected Unit parent;

    protected GameObject hitTarget;       //範囲内に入ったターゲット
    protected bool hitFlag = false;

    void OnTriggerEnter(Collider collider)
    {
        if (parent != null)
            //目標が範囲内に入ってきたとき
            if (collider.gameObject == parent.targetObject)
            {
                hitTarget = collider.gameObject;
                hitFlag = true;
            }
    }

    void OnTriggerStay(Collider collider)
    {
        if(parent != null)
            //目標が範囲内に入っているとき
            if (collider.gameObject == parent.targetObject)
            {
                hitTarget = collider.gameObject;
                hitFlag = true;
            }
    }

    void OnTriggerExit(Collider collider)
    {
        if (parent != null)
            //目標が範囲内から出たとき
            if (collider.gameObject == parent.targetObject)
            {
                hitTarget = null;
                hitFlag = false;
            }
    }
}
