using UnityEngine;
using StaticClass;
using System.Collections;
using System.Collections.Generic;

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

    //状態
    public enum State
    {
        Search,
        Find,
        Attack,
        Dead,
        Wait,
        ALLAttack
    }
    //[HideInInspector]
    public State state = State.Wait;

    //ステータス
    [SerializeField, TooltipAttribute("ステータス")]
    public Status status;
    //[HideInInspector]
    public int level = 0;
    //攻撃範囲
    [SerializeField]
    public float ATKRange = 10.0f;
    //徘徊速度
    [SerializeField]
    public float loiteringSPEED = 1.0f;

    [HideInInspector]
    public bool IsDead;
    [HideInInspector]
    public bool IsDamage;   //ダメージ確認
    protected int oldHP = 0;    //直前の体力確認用

    [HideInInspector]
    public GameObject goalObject;       //ゴール
    [HideInInspector]
    public GameObject targetObject;       //目標
    protected float targetDistance = 0.0f;
    protected float targetColliderRadius = 0.0f;

    [HideInInspector]
    public List<GameObject> allTargetObject;       //複数攻撃目標

    public GameObject deadEffect;       //死亡エフェクト
    
    [HideInInspector]
    public string targetTag;       //相手のタグ

    //巡回地点
    protected Transform[] loiteringPointObj;
    public Transform[] LoiteringPointObj { set { loiteringPointObj = value; } }

    int currentRootPoint = 0;
    [HideInInspector]
    public Enum.Direction_TYPE rootNum = Enum.Direction_TYPE.Bottom;

    [SerializeField]
    protected float deadMoveSpeed = 0.0f;    //死んだときに動くならその値
    [SerializeField]
    protected float deadTime = 1.0f;
    protected float deadcount = 0.0f;

    //出現場所の目的場所
    Vector3 spawnTargetPosition;
    public Vector3 SpawnTargetPosition
    {
        get { return spawnTargetPosition; }
        set { spawnTargetPosition = value; }
    }

    //無敵時間
    [SerializeField]
    protected float invincibleTime = 1.3f;
    protected bool invincibleFlag = false;
    
    //使うコンポーネント
    protected UnitSeach seach;
    protected UnitAttack attack;
    protected UnitMove move;
    protected SphereCollider sCollider;

    public void Move(Vector3 target, float speed)
    {
        //NavMeshAgentで動かす
        if (GetComponent<NavMeshAgent>())
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            //agent.speed = speed * 8.0f;
            //agent.speed = 50.0f;        //全員一律で同じ速さで壁を回避
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

        //一番近い敵との直線距離
        targetDistance = Vector3.Distance(this.transform.position, targetObject.transform.position);
        if (targetObject.GetComponent<SphereCollider>())
            targetColliderRadius = targetObject.GetComponent<SphereCollider>().radius * targetObject.transform.localScale.x;
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
        
        return rootPosition;
    }

    //ポイントを通過するための更新
    public void UpdataRootPoint(float distance)
    {
        if (Vector3.Distance(transform.position, loiteringPointObj[currentRootPoint].transform.position) < distance)
        {
            if (currentRootPoint < loiteringPointObj.Length - 1)
                currentRootPoint++;
        }
    }

    protected IEnumerator NearTarget()
    {
        while (true)
        {
            //一番近くの敵を狙う
            SetNearTargetObject();

            yield return new WaitForSeconds(0.4f);
        }
    }

}