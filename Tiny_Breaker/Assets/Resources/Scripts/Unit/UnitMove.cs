using UnityEngine;
using System.Collections;

public class UnitMove : MonoBehaviour
{
    Coroutine cor;
    
    IEnumerator Move()
    {
        Unit unit = gameObject.GetComponent<Unit>();

        RaycastHit hit;
        int layerMask = ~(1 << transform.gameObject.layer);  //自身のレイヤー番号以外にヒットするようにしたビット演算

        //動くときには回れるように
        gameObject.GetComponent<Rigidbody>().freezeRotation = false;

        while (true)
        {
            if (unit.targetObject != null)
            {
                switch (unit.state)
                {
                    case Unit.State.Search:
                        Vector3 rootPos = unit.GetRootPosition();
                        Vector3 rootVec = rootPos - transform.position;
                        transform.LookAt(transform.position + rootVec);

                        //サーチ中止まってたら何かに引っかかってる可能性
                        Ray raySearch = new Ray(transform.position, rootVec);
                        if (Physics.SphereCast(raySearch, 3.0f, out hit, 5.0f, layerMask))
                        {
                            if (hit.collider.gameObject.tag == "Ground")
                            {
                                //ナビゲーションは最低限で用いる
                                GetComponent<NavMeshAgent>().enabled = true;
                                unit.Move(rootPos, unit.loiteringSPEED);
                            }
                            else
                            {
                                GetComponent<NavMeshAgent>().enabled = false;
                                GetComponent<Rigidbody>().velocity = rootVec.normalized * unit.loiteringSPEED;
                            }
                        }
                        else
                        {
                            GetComponent<NavMeshAgent>().enabled = false;
                            GetComponent<Rigidbody>().velocity = rootVec.normalized * unit.loiteringSPEED;
                        }
                        break;
                    case Unit.State.Find:
                        Vector3 targetVec = unit.targetObject.transform.position - transform.position;
                        transform.LookAt(transform.position + targetVec);

                        //サーチ中止まってたら何かに引っかかってる可能性
                        Ray rayFind = new Ray(transform.position, targetVec);
                        if (Physics.SphereCast(rayFind, 3.0f, out hit, 5.0f, layerMask))
                        {
                            if (hit.collider.gameObject.tag == "Ground")
                            {
                                //ナビゲーションは最低限で用いる
                                GetComponent<NavMeshAgent>().enabled = true;
                                unit.Move(unit.targetObject.transform.position, unit.status.CurrentSPEED);
                            }
                            else
                            {
                                GetComponent<NavMeshAgent>().enabled = false;
                                GetComponent<Rigidbody>().velocity = targetVec.normalized * unit.status.CurrentSPEED;
                            }
                        }
                        else
                        {
                            GetComponent<NavMeshAgent>().enabled = false;
                            GetComponent<Rigidbody>().velocity = targetVec.normalized * unit.status.CurrentSPEED;
                        }
                        break;
                    default:
                        GetComponent<NavMeshAgent>().enabled = false;
                        GetComponent<Rigidbody>().velocity = Vector3.zero;
                        break;
                }
            }
            else
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            
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