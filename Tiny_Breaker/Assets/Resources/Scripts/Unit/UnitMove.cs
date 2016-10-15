using UnityEngine;
using System.Collections;

public class UnitMove : MonoBehaviour
{
    Coroutine cor;

    [SerializeField]
    float spawnMoveTime = 0.0f;
    [SerializeField]
    float spawnStopTime = 0.0f;

    void Start()
    {
        cor = StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        Unit unit = gameObject.GetComponent<Unit>();

        RaycastHit hit;
        int layerMask = ~(1 << transform.gameObject.layer);  //自身のレイヤー番号以外にヒットするようにしたビット演算

        //動くときには回れるように
        gameObject.GetComponent<Rigidbody>().freezeRotation = false;

        //召喚時の動き
        bool spawnEnd = false;
        Vector3 startPosition = transform.position;
        Vector3 subVec = unit.spawnTargetPosition - startPosition;
        float time = 0.0f;
        while (spawnEnd == false)
        {
            time += Time.deltaTime;
            if (time >= spawnMoveTime)
            {
                spawnEnd = true;
                time = spawnMoveTime;
            }
            float rate = time / spawnMoveTime;
            Vector3 rateVec = subVec * rate;
            transform.position = startPosition + rateVec;
            GetComponent<NavMeshAgent>().enabled = false;
            yield return null;
        }
        yield return new WaitForSeconds(spawnStopTime);

        //通常の時の動き
        while (true)
        {
            if (unit.targetObject != null)
            {
                switch (unit.state)
                {
                    case Unit.State.Search:
                        unit.UpdataRootPoint(3.0f);

                        Vector3 rootPos = unit.GetRootPosition();
                        Vector3 rootVec = rootPos - transform.position;
                        transform.LookAt(transform.position + rootVec);

                        //サーチ中止まってたら何かに引っかかってる可能性
                        Ray raySearch = new Ray(transform.position, rootVec);
                        if (Physics.SphereCast(raySearch, 3.0f, out hit, 5.0f, layerMask))
                        {
                            if (hit.collider.gameObject.tag == "Ground" || hit.collider.gameObject.layer == 9)
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
                        unit.UpdataRootPoint(15.0f);
                        //重なっている
                        if (Vector3.Distance(this.transform.position, unit.targetObject.transform.position) < 3.0f) //3.0f = 重なっているとする距離
                        {
                            GetComponent<NavMeshAgent>().enabled = false;
                            GetComponent<Rigidbody>().velocity = Vector3.zero;
                        }
                        else
                        { 
                            Vector3 targetVec = unit.targetObject.transform.position - transform.position;
                            transform.LookAt(transform.position + targetVec);

                            //サーチ中止まってたら何かに引っかかってる可能性
                            Ray rayFind = new Ray(transform.position, targetVec);
                            if (Physics.SphereCast(rayFind, 3.0f, out hit, 5.0f, layerMask))
                            {
                                if (hit.collider.gameObject.tag == "Ground" || hit.collider.gameObject.layer == 9)
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
        //cor = StartCoroutine(Move());
    }

    void OnDisable()
    {
        StopCoroutine(cor);
    }
}