using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

    //ステータス
    [SerializeField, TooltipAttribute("ステータス")]
    public Status status;

    public bool IsDaed;
    
    //攻撃関連
    [HideInInspector]
    public bool IsAttack;           //攻撃中フラグ
    [HideInInspector]
    public GameObject targetObject;       //目標

    protected void Move(GameObject target)
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //ターゲットがいない場合
        if (target == null)
        {
            //NavMeshAgentを止める
            if (GetComponent<NavMeshAgent>())
            {
                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                agent.destination = this.transform.position;
            }
            return;
        }

        Vector3 targetPosition = target.transform.position;
        //目的地への方向を見る
        transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
        
        if (!IsAttack)
        {
            //NavMeshAgentで動かす
            if (GetComponent<NavMeshAgent>())
            {
                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                agent.speed = status.CurrentSPEED;
                agent.destination = targetPosition;
            }
        }
        else
        {
            //NavMeshAgentで動かす
            if (GetComponent<NavMeshAgent>())
            {
                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                agent.destination = this.transform.position;
            }
        }
    }
}
