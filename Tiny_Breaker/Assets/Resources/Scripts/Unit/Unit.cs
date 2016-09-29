using UnityEngine;
using StaticClass;

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

    public enum State
    {
        Search,
        Find,
        Attack,
        Dead,
        Wait
    }
    public State state = State.Wait;

    //ステータス
    [SerializeField, TooltipAttribute("ステータス")]
    public Status status;

    [SerializeField]
    public float ATKRange = 10.0f;

    [SerializeField]
    public float loiteringSPEED = 1.0f;

    [HideInInspector]
    public bool IsDead;
    [HideInInspector]
    public bool IsDamage;   //ダメージ確認
    protected int oldHP = 0;    //直前の体力確認用

    //[HideInInspector]
    public GameObject goalObject;       //ゴール
    //[HideInInspector]
    public GameObject targetObject;       //目標

    public GameObject deadEffect;       //死亡エフェクト

    public GameObject deadSE;           //遊び

    //[HideInInspector]
    public string targetTag;       //相手のタグ

    //巡回地点
    protected Transform[] loiteringPointObj;
    public Transform[] LoiteringPointObj { set { loiteringPointObj = value; } }

    int currentRootPoint = 0;
    //[HideInInspector]
    public int rootNum = 0;

    public void Move(Vector3 target, float speed)
    {
        //NavMeshAgentで動かす
        if (GetComponent<NavMeshAgent>())
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.speed = speed;
            agent.destination = target;
        }
    }
    
    public void SetNearTargetObject()
    {
        //プレイヤーのTarget
        targetObject = goalObject;

        //悪魔
        GameObject nearestObject = DemonDataBase.getInstance().GetNearestObject(targetTag, this.transform.position, rootNum);
        GameObject nearSol = SolgierDataBase.getInstance().GetNearestObject(transform.gameObject.tag, this.transform.position, rootNum);
        GameObject nearBuild = BuildingDataBase.getInstance().GetNearestObject(this.transform.position);
        
        if (nearestObject != null && targetObject != null)
            if (nearestObject.tag != transform.gameObject.tag)
                if (Vector3.Distance(this.transform.position, nearestObject.transform.position) <
                    Vector3.Distance(this.transform.position, targetObject.transform.position))
                    targetObject = nearestObject;

        if (nearSol != null && targetObject != null)
            if (nearSol.tag != transform.gameObject.tag)
                if (Vector3.Distance(this.transform.position, nearSol.transform.position) <
                    Vector3.Distance(this.transform.position, targetObject.transform.position))
                    targetObject = nearSol;

        if (nearBuild != null && targetObject != null)
            if (nearBuild.tag != transform.gameObject.tag)
                if (Vector3.Distance(this.transform.position, nearBuild.transform.position) <
                    Vector3.Distance(this.transform.position, targetObject.transform.position))
                    targetObject = nearBuild;
    }

    //ダメージを受けたか確認する
    public void DamageCheck(int nowHP)
    {
        if(nowHP < oldHP)
        {
            IsDamage = true;
        }
        else
        {
            IsDamage = false;
        }
        oldHP = nowHP;
    }

    //巡回ルートの座標を所得する
    public Vector3 GetRootPosition()
    {
        Vector3 rootPosition = loiteringPointObj[currentRootPoint].transform.position;
        
        if (Vector3.Distance(transform.position, rootPosition) < 5.0f)
        {
            if (currentRootPoint < loiteringPointObj.Length - 1)
                currentRootPoint++;
        }

        return rootPosition;
    }
}
