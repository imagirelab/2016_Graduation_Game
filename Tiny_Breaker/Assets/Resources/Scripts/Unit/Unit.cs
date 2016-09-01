using UnityEngine;

public class Unit : MonoBehaviour
{
    //タイプ
    public enum Type
    {
        Blue,
        Red,
        Green,
        White
    }
    public Type type = Type.White;

    //ステータス
    [SerializeField, TooltipAttribute("ステータス")]
    public Status status;

    [SerializeField]
    public float loiteringSPEED = 1.0f;

    [HideInInspector]
    public bool IsDead;

    //攻撃関連
    [HideInInspector]
    public bool IsAttack;           //攻撃中フラグ
    [HideInInspector]
    public bool IsFind;             //発見フラグ
    [HideInInspector]
    public GameObject targetObject;       //目標
    [HideInInspector]
    public GameObject goalObject;       //ゴール

    [HideInInspector]
    public string targetTag;       //相手のタグ

    //巡回地点
    protected Transform[] loiteringPointObj;
    public Transform[] LoiteringPointObj { set { loiteringPointObj = value; } }

    int currentRoot = 0;

    protected void Move(GameObject target)
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //NavMeshAgentを止める
        if (GetComponent<NavMeshAgent>())
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = transform.position;
        }

        //ターゲットがいない場合
        if (target == null)
            return;

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
                //agent.Resume();
            }
        }
        else
        {
            //NavMeshAgentで動かす
            if (GetComponent<NavMeshAgent>())
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                agent.destination = transform.position;
            }
        }
    }

    //徘徊命令
    protected void Loitering(Transform[] LoiteringPos)
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //NavMeshAgentを止める
        if (GetComponent<NavMeshAgent>())
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.Stop();
        }

        Vector3 targetPosition = LoiteringPos[currentRoot].transform.position;
        //目的地への方向を見る
        transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));

        if (!IsFind)
        {
            //NavMeshAgentで動かす
            if (GetComponent<NavMeshAgent>())
            {
                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                agent.speed = loiteringSPEED;
                agent.destination = targetPosition;
                agent.Resume();
            }
        }

        if (Vector3.Distance(transform.position, targetPosition) < 5.0f)
        {
            //巡回しない方
            if (GetComponent<NavMeshAgent>())
            {
                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                agent.destination = transform.position;
            }
            if (currentRoot < LoiteringPos.Length - 1)
                currentRoot++;
            //currentRoot = (currentRoot < LoiteringPos.Length - 1) ? currentRoot + 1 : 0;
        }
    }
}
