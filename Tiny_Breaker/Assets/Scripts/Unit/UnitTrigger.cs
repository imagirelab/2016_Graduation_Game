//Unitに使える、
//Triggerの範囲内に入った対象に何かをする時の親クラス

using UnityEngine;

public class UnitTrigger : MonoBehaviour
{
    //Unitクラスの親
    protected Unit parent;

    protected GameObject hitTarget;       //範囲内に入ったターゲット
    protected bool hitFlag = false;

    void OnTriggerEnter(Collider collider)
    {
        if (parent == null)
            return;
        
        if (parent.targetObject != null)
        {
            //目標が範囲内に入っているとき
            if (collider.gameObject == parent.targetObject)
            {
                //レイが通ったら当たる
                Vector3 subTargetPosition = parent.targetObject.transform.position - transform.position;
                Ray ray = new Ray(transform.position, subTargetPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, subTargetPosition.magnitude))
                {
                    if (hit.collider.gameObject == parent.targetObject)
                    {
                        hitTarget = collider.gameObject;
                        hitFlag = true;
                    }
                }

                //何かしら入ってはいたけど目的ではなくなっていたら
                if (hitTarget != null)
                {
                    if (hitTarget != parent.targetObject)
                    {
                        hitTarget = null;
                        hitFlag = false;
                    }
                }
            }
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (parent == null)
            return;

        if (parent.targetObject != null)
        {
            //目標が範囲内に入っているとき
            if (collider.gameObject == parent.targetObject)
            {
                //レイが通ったら当たる
                Vector3 subTargetPosition = parent.targetObject.transform.position - transform.position;
                Ray ray = new Ray(transform.position, subTargetPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, subTargetPosition.magnitude))
                {
                    if (hit.collider.gameObject == parent.targetObject)
                    {
                        hitTarget = collider.gameObject;
                        hitFlag = true;
                    }
                }
            }

            //何かしら入ってはいたけど目的ではなくなっていたら
            if (hitTarget != null)
            {
                if (hitTarget != parent.targetObject)
                {
                    hitTarget = null;
                    hitFlag = false;
                }
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        //目標が範囲内から出たとき
        if (collider.gameObject == hitTarget)
        {
            hitTarget = null;
            hitFlag = false;
        }
    }
}
