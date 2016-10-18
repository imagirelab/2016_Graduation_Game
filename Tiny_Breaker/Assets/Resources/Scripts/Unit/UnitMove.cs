using UnityEngine;
using System.Collections;

public class UnitMove : MonoBehaviour
{
    Coroutine cor;

    [SerializeField]
    GameObject statusUI;    //召喚中に消すステータスのUI
    [SerializeField]
    float spawnMoveTime = 0.0f;
    [SerializeField]
    float spawnStopTime = 0.0f;
    [SerializeField]
    float setTime = 0.0f;
    bool setFlag = false;   //構えたどうかのフラグ

    void Start()
    {
        cor = StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        Unit unit = gameObject.GetComponent<Unit>();

        RaycastHit hit;
        int layerMask = ~(1 << transform.gameObject.layer);  //自身のレイヤー番号以外にヒットするようにしたビット演算
        
        //召喚時の動きがあるもの
        if(unit.setSpawnTargetFlag)
        {
            //ステータスUIを消す
            if (statusUI == null)
                statusUI = new GameObject();
            statusUI.SetActive(false);

            bool spawnEnd = false;
            Vector3 startPosition = transform.position;
            Vector3 subVec;
            subVec = unit.SpawnTargetPosition - startPosition;
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
                transform.position = startPosition + rateVec;   //座標の代入
                //GetComponent<Rigidbody>().velocity = subVec.normalized * 30.0f;
                GetComponent<NavMeshAgent>().enabled = false;
                yield return null;
            }
            yield return new WaitForSeconds(spawnStopTime);
            //ステータスUIを表示
            statusUI.SetActive(true);
        }
        GetComponent<SphereCollider>().enabled = true;
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;

        //通常の時の動き
        while (true)
        {
            if (unit.targetObject != null)
            {
                switch (unit.state)
                {
                    case Unit.State.Search:
                        //構えの処理
                        if (!setFlag)
                        {
                            //GetComponent<NavMeshAgent>().enabled = false;
                            GetComponent<Rigidbody>().velocity = Vector3.zero;
                            yield return new WaitForSeconds(setTime);
                            setFlag = true;
                        }

                        unit.UpdataRootPoint(1.0f);

                        Vector3 rootPos = unit.GetRootPosition();
                        Vector3 rootVec = rootPos - transform.position;
                        transform.LookAt(transform.position + rootVec);

                        //サーチ中止まってたら何かに引っかかってる可能性
                        //Ray raySearch = new Ray(transform.position, rootVec);
                        Ray raySearch = new Ray(new Vector3(
                            transform.position.x,
                            transform.position.y + 1.5f,    //視線の高さ分上げている形
                            transform.position.z),
                            rootPos);

                        if (Physics.SphereCast(raySearch, 3.0f, out hit, 3.0f, layerMask))
                        {
                            if (hit.collider.gameObject.tag == "Ground" ||
                                hit.collider.gameObject.layer == 9 ||
                                hit.collider.gameObject.layer == 14)
                            {
                                //ナビゲーションは最低限で用いる
                                GetComponent<NavMeshAgent>().enabled = true;
                                unit.Move(rootPos, unit.loiteringSPEED);
                                Debug.Log(hit.collider.gameObject.name);
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
                        setFlag = false;

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
                            //Ray rayFind = new Ray(transform.position, targetVec);
                            Ray rayFind = new Ray(new Vector3(
                            transform.position.x,
                            transform.position.y + 1.5f,    //視線の高さ分上げている形
                            transform.position.z),
                            targetVec);

                            if (Physics.SphereCast(rayFind, 3.0f, out hit, 3.0f, layerMask))
                            {
                                if (hit.collider.gameObject.tag == "Ground" ||
                                    hit.collider.gameObject.layer == 9 ||
                                    hit.collider.gameObject.layer == 14)
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
                        setFlag = false;
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