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

    //unit.setSpawnTargetFlagを読み取るためStart()からスタート
    void Start()
    {
        cor = StartCoroutine(Move());
    }
    
    IEnumerator Move()
    {
        Unit unit = gameObject.GetComponent<Unit>();
        
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
                    case Unit.State.Find:
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
    
    void OnDisable()
    {
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