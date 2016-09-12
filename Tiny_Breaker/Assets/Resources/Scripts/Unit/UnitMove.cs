using UnityEngine;
using System.Collections;

public class UnitMove : MonoBehaviour
{
    bool canMoveing = true;

    Coroutine cor;
    
    IEnumerator Move()
    {
        Unit unit = gameObject.GetComponent<Unit>();

        while (true)
        {
            //基本動く
            canMoveing = true;
            //攻撃中だけ立ち止まる
            if (unit.IsAttack)
                canMoveing = false;

            if (canMoveing)
            {
                //ターゲットがいない場合
                if (unit.targetObject != null)
                {
                    Vector3 targetPosition = unit.targetObject.transform.position;
                    transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));

                    gameObject.GetComponent<Rigidbody>().velocity = targetPosition.normalized * unit.status.CurrentSPEED;
                }
            }
            else
            {
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            
            yield return null;
        }
    }

    void OnEnable()
    {
        cor = StartCoroutine(Move());
    }

    void OnDisable()
    {
        StopCoroutine(cor);
    }
}