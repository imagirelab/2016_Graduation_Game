using UnityEngine;
using System.Collections;

public class UnitMove : MonoBehaviour
{
    Coroutine cor;
    
    IEnumerator Move()
    {
        Unit unit = gameObject.GetComponent<Unit>();

        //動くときには回れるように
        gameObject.GetComponent<Rigidbody>().freezeRotation = false;

        while (true)
        {
            if (unit.targetObject != null)
            {
                RaycastHit hit;

                switch (unit.state)
                {
                    case Unit.State.Search:

                        Vector3 rootPos = unit.GetRootPosition();
                        Vector3 rootVec = rootPos - transform.position;
                        transform.LookAt(transform.position + rootVec);

                        //サーチ中、止まってたら何かに引っかかってる可能性
                        Ray raySearch = new Ray(transform.position, rootVec);
                        if (Physics.SphereCast(raySearch, 2.0f, out hit, 5.0f))
                        {
                            //ナビゲーションは最低限で用いる
                            gameObject.GetComponent<NavMeshAgent>().enabled = true;
                            unit.Move(rootPos, unit.loiteringSPEED);
                        }
                        else
                        {
                            gameObject.GetComponent<NavMeshAgent>().enabled = false;
                            gameObject.GetComponent<Rigidbody>().velocity = rootVec.normalized * unit.loiteringSPEED;
                        }
                        break;
                    case Unit.State.Find:

                        Vector3 targetVec = unit.targetObject.transform.position - transform.position;
                        transform.LookAt(transform.position + targetVec);

                        //サーチ中、止まってたら何かに引っかかってる可能性
                        Ray rayFind = new Ray(transform.position, targetVec);
                        if (Physics.SphereCast(rayFind, 2.0f, out hit, 5.0f))
                        {
                            //ナビゲーションは最低限で用いる
                            gameObject.GetComponent<NavMeshAgent>().enabled = true;
                            unit.Move(unit.targetObject.transform.position, unit.status.CurrentSPEED);
                        }
                        else
                        {
                            gameObject.GetComponent<NavMeshAgent>().enabled = false;
                            gameObject.GetComponent<Rigidbody>().velocity = targetVec.normalized * unit.status.CurrentSPEED;
                        }
                        break;
                    default:
                        gameObject.GetComponent<NavMeshAgent>().enabled = false;
                        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        break;
                }
            }
            else
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            
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