using UnityEngine;
using System.Collections;

public class UnitMove : MonoBehaviour
{
    Coroutine cor;

    [SerializeField]
    float setTime = 0.0f;
    bool setFlag = false;   //構えたどうかのフラグ

    //unit.setSpawnTargetFlagを読み取るためStart()からスタート
    //void Start()
    //{
    //    cor = StartCoroutine(Move());
    //}
    
    IEnumerator Move()
    {
        Unit unit = gameObject.GetComponent<Unit>();
        
        //通常の時の動き
        while (true)
        {
            if (unit.targetObject != null)
            {
                switch (unit.state)
                {
                    case Enum.State.Search:
                        //構えの処理
                        if (!setFlag)
                        {
                            UnitStop();
                            yield return new WaitForSeconds(setTime);
                            setFlag = true;
                        }

                        unit.UpdataRootPoint(1.0f);
                        //重なっている
                        if (Vector3.Distance(this.transform.position, unit.targetObject.transform.position) < 3.0f) //3.0f = 重なっているとする距離
                            UnitStop();
                        else
                        {
                            Vector3 rootPos = unit.GetRootPosition();
                            Vector3 rootVec = rootPos - transform.position;
                            transform.LookAt(transform.position + rootVec);
                            
                            if (GetComponent<NavMeshAgent>().enabled)
                                unit.Move(rootPos, unit.loiteringSPEED);
                            else
                                GetComponent<Rigidbody>().velocity = rootVec.normalized * unit.loiteringSPEED;
                        }
                        break;
                    case Enum.State.Find:
                        setFlag = false;

                        unit.UpdataRootPoint(15.0f);
                        //重なっている
                        if (Vector3.Distance(this.transform.position, unit.targetObject.transform.position) < 3.0f) //3.0f = 重なっているとする距離
                            UnitStop();
                        else
                        {
                            Vector3 targetVec = unit.targetObject.transform.position - transform.position;
                            transform.LookAt(transform.position + targetVec);
                            
                            if (GetComponent<NavMeshAgent>().enabled)
                                unit.Move(unit.targetObject.transform.position, unit.status.CurrentSPEED);
                            else
                                GetComponent<Rigidbody>().velocity = targetVec.normalized * unit.status.CurrentSPEED;
                        }
                        break;
                    default:
                        setFlag = false;
                        UnitStop();
                        break;
                }
            }
            else
                UnitStop();

            yield return null;
        }
    }

    void OnEnable()
    {
        cor = StartCoroutine(Move());
    }

    void OnDisable()
    {
        if(cor != null)
            StopCoroutine(cor);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        GetComponent<NavMeshAgent>().enabled = true;
    }

    void OnCollisionStay(Collision collision)
    {
        GetComponent<NavMeshAgent>().enabled = true;
    }

    void OnCollisionExit(Collision collision)
    {
        UnitStop();
    }

    //移動を止める
    void UnitStop()
    {
        if (GetComponent<NavMeshAgent>())
            GetComponent<NavMeshAgent>().speed = 0.0f;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}