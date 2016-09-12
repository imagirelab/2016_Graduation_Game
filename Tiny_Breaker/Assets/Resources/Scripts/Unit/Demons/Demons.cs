// 悪魔単体の処理をするクラス

using UnityEngine;
using StaticClass;

public class Demons : Unit
{
    //成長値
    [SerializeField, TooltipAttribute("悪魔の成長値ポイント")]
    private GrowPoint growPoint;
    public GrowPoint GrowPoint
    {
        get { return growPoint; }
        set { growPoint = value; }
    }

    void Start()
    {
        // 作られたときにリストに追加する
        DemonDataBase.getInstance().AddList(this.gameObject, transform.gameObject.tag);

        //死亡フラグ
        IsDead = false;

        //巡回ルート
        if (gameObject.transform.parent == null)
            loiteringPointObj = new Transform[] { transform };

        //ステータスの決定
        SetStatus();
    }

    void Update()
    {
        //終わり
        //if (transform.parent.gameObject.GetComponent<Player>().target == null)
        //{
        //    Wait();
        //    return;
        //}


        //見つける
        if (targetObject != null)
        {
            if (Vector3.Distance(transform.position, targetObject.transform.position) < 40.0f)  //40.0f = Collider.Radius * LocalScale
            {
                IsFind = true;
            }
            else
            {
                IsFind = false;
            }

            //攻撃
            if (Vector3.Distance(transform.position, targetObject.transform.position) < 10.0f)
            {
                IsAttack = true;
                gameObject.GetComponent<UnitAttack>().enabled = true;
                gameObject.GetComponent<UnitMove>().enabled = false;
            }
            else
            {
                IsAttack = false;
                gameObject.GetComponent<UnitAttack>().enabled = false;
                gameObject.GetComponent<UnitMove>().enabled = true;
            }
        }

            //プレイヤーのTarget
            //targetObject = GetNearTargetObject();

            ////攻撃対象の設定
            //if (transform.parent != null)
            //{
            //    //プレイヤーのTarget
            //    targetObject = goalObject;

            //    //悪魔
            //    GameObject nearestObject = DemonDataBase.getInstance().GetNearestObject(targetTag, this.transform.position);
            //    if (nearestObject != null)
            //        if (Vector3.Distance(this.transform.position, nearestObject.transform.position) <
            //                Vector3.Distance(this.transform.position, targetObject.transform.position))
            //            targetObject = nearestObject;

            //    //兵士
            //    GameObject nearSol = SolgierDataBase.getInstance().GetNearestObject(this.transform.position);
            //    if (nearSol != null)
            //        if (nearSol.tag != transform.gameObject.tag)
            //            if (Vector3.Distance(this.transform.position, nearSol.transform.position) <
            //                    Vector3.Distance(this.transform.position, targetObject.transform.position))
            //                targetObject = nearSol;

            //    //建物
            //    GameObject nearBuild = BuildingDataBase.getInstance().GetNearestObject(this.transform.position);
            //    if (nearBuild != null)
            //        if (nearBuild.tag != transform.gameObject.tag)
            //            if (Vector3.Distance(this.transform.position, nearBuild.transform.position) <
            //                    Vector3.Distance(this.transform.position, targetObject.transform.position))
            //                targetObject = nearBuild;
            //}

            ////見つける距離
            //if ((transform.position - targetObject.transform.position).sqrMagnitude < 40.0f * 40.0f)  //40.0f = Collider.Radius * LocalScale
            //{
            //    Debug.Log("Find");
            //}
            ////攻撃する距離
            //if (Vector3.Distance(transform.position, targetObject.transform.position) < 8.0f)
            //{
            //    Debug.Log("Attack");
            //}

            //if (!IsFind)
            //    Loitering(loiteringPointObj);
            //else
            //Move(targetObject);

            //死亡処理
            if (status.CurrentHP <= 0)
            Dead();
    }
    
    //待機指示
    void Wait()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //NavMeshAgentを止める
        if (GetComponent<NavMeshAgent>())
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = this.transform.position;
        }
    }

    //死んだときの処理
    void Dead()
    {
        IsDead = true;

        //死んだ直後に魂を回収してみる
        if (transform.parent != null)
            transform.parent.gameObject.GetComponent<Player>().AddSpiritList(growPoint);
        
        Destroy(gameObject);
    }

    //ステータスの設定
    public void SetStatus()
    {
        //基礎ステータスの代入
        status.SetStatus();

        //今のステータスを算出する
        for (int i = 0; i < growPoint.CurrentHP_GrowPoint - growPoint.GetHP_GrowPoint; i++)
            status.CurrentHP += (int)(status.GetHP * 0.5f);
        for (int i = 0; i < growPoint.CurrentATK_GrowPoint - growPoint.GetATK_GrowPoint; i++)
            status.CurrentATK += (int)(status.GetATK * 0.5f);
        for (int i = 0; i < growPoint.CurrentSPEED_GrowPoint - growPoint.GetSPEED_GrowPoint; i++)
            status.CurrentSPEED += status.GetSPEED * 0.15f;
        for (int i = 0; i < growPoint.CurrentAtackTime_GrowPoint - growPoint.GetAtackTime_GrowPoint; i++)
            status.CurrentAtackTime -= status.GetAtackTime * 0.05f;

        //カンスト
        if (status.CurrentHP >= 9999)
            status.CurrentHP = 9999;
        if (status.CurrentATK >= 2000)
            status.CurrentATK = 2000;
        if (status.CurrentSPEED >= 10)
            status.CurrentSPEED = 10;
        if (status.CurrentAtackTime <= 0.5f)    //１フレーム以下にならない方がいいかも
            status.CurrentAtackTime = 0.5f;

        status.MaxHP = status.CurrentHP;

        loiteringSPEED = status.CurrentSPEED;
    }

    //破壊されたときにリストから外す
    void OnDisable()
    {
        DemonDataBase.getInstance().RemoveList(this.gameObject);
    }

}
