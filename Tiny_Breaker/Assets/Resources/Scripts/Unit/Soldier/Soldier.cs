// 敵のステータスなどを管理するクラス

using UnityEngine;
using StaticClass;

public class Soldier : Unit
{
    enum State
    {
        Dead,
        Find,
        Search,
        Wait
    }
    State state = State.Wait;

    [SerializeField]
    float deadTime = 1.0f;
    float deadcount = 0.0f;

    public int powerUPCount = 0;

    void Start()
    {
        // 作られたときにリストに追加する
        SolgierDataBase.getInstance().AddList(this.gameObject, transform.gameObject.tag);

        //状態の設定
        state = State.Wait;

        //ステータスの設定
        SetStatus();

        deadcount = 0.0f;

        if (gameObject.transform.parent == null)
            loiteringPointObj = new Transform[] { transform };
    }

    void Update()
    {
        //死んだときの処理
        if (status.CurrentHP <= 0)
            Dead();

        //状態の変更条件
        if (IsDead)
            state = State.Dead;
        else
        {
            if (goalObject == null)
                state = State.Wait;
            else
            {
                if (IsFind)
                    state = State.Find;
                else
                    state = State.Search;
            }
        }

        //攻撃対象の設定
        if (transform.parent != null)
        {
            //プレイヤーのTarget
            targetObject = goalObject;

            //悪魔
            GameObject nearestObject = DemonDataBase.getInstance().GetNearestObject(targetTag, this.transform.position);
            if (nearestObject != null)
                if (Vector3.Distance(this.transform.position, nearestObject.transform.position) <
                        Vector3.Distance(this.transform.position, targetObject.transform.position))
                    targetObject = nearestObject;

            //兵士
            GameObject nearSol = SolgierDataBase.getInstance().GetNearestObject(targetTag, this.transform.position);
            if (nearSol != null)
                if (nearSol.tag != transform.gameObject.tag)
                    if (Vector3.Distance(this.transform.position, nearSol.transform.position) <
                            Vector3.Distance(this.transform.position, targetObject.transform.position))
                        targetObject = nearSol;

            //建物
            GameObject nearBuild = BuildingDataBase.getInstance().GetNearestObject(this.transform.position);
            if (nearBuild != null)
                if (nearBuild.tag != transform.gameObject.tag)
                    if (Vector3.Distance(this.transform.position, nearBuild.transform.position) <
                            Vector3.Distance(this.transform.position, targetObject.transform.position))
                        targetObject = nearBuild;
        }

        //Debug.Log(IsFind);
        //Debug.Log(IsAttack);

        //状態ごとの処理
        switch (state)
        {
            case State.Dead:
                Dying();
                break;
            case State.Find:
                Find();
                break;
            case State.Search:
                Search();
                break;
            default:
                Wait();
                break;
        }
    }

    //発見時処理
    void Find()
    {
        //移動
        Move(targetObject);
    }

    //索敵処理
    void Search()
    {
        ////見つけたいもの
        //if (IsDefenseBase)
        //    targetObject = defenseBase;
        //else
        //targetObject = DemonDataBase.getInstance().GetNearestObject(transform.position);

        Loitering(loiteringPointObj);
    }

    //待機処理　一応
    void Wait()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        //NavMeshAgentを止める
        if (GetComponent<NavMeshAgent>())
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = transform.position;
        }
    }

    //死にかけている時の処理
    void Dying()
    {
        deadcount += Time.deltaTime;

        if (deadcount > deadTime)
            Destroy(gameObject);
    }

    //死んだときの処理
    void Dead()
    {
        if (!IsDead)
        {
            IsDead = true;

            //リストから外すタイミングを死んだ条件の中に入れる
            SolgierDataBase.getInstance().RemoveList(this.gameObject);

            //いらない子供から消していく
            if (transform.IsChildOf(transform))
                foreach (Transform child in transform)
                {
                    if (child.name == "Models")
                    {
                        foreach (Component comp in child.GetComponents<Component>())
                            if (comp != child.GetComponent<Transform>())
                                Destroy(comp);

                        foreach (Transform grandson in child)
                            if (grandson.name == "Model")
                            {
                                foreach (Component comp in grandson.GetComponents<Component>())
                                    if (comp != grandson.GetComponent<Transform>())
                                        Destroy(comp);

                                foreach (GameObject e in GetAllChildren.GetAll(grandson.gameObject))
                                    if (e.GetComponent<Collider>())
                                    {
                                        e.GetComponent<Collider>().enabled = true;
                                        e.AddComponent<Rigidbody>();
                                    }
                            }
                    }
                    else
                        Destroy(child.gameObject);
                }

            foreach (Component comp in this.GetComponents<Component>())
                if (comp != GetComponent<Transform>() && comp != GetComponent<Soldier>())
                    Destroy(comp);
        }
    }

    //ステータスの設定
    void SetStatus()
    {
        //基礎ステータスの代入
        status.SetStatus();

        //今のステータスを算出する
        for (int i = 0; i < powerUPCount; i++)
            status.CurrentHP += (int)(status.GetHP * 0.5f);
        for (int i = 0; i < powerUPCount; i++)
            status.CurrentATK += (int)(status.GetATK * 0.5f);
        for (int i = 0; i < powerUPCount; i++)
            status.CurrentSPEED += status.GetSPEED * 0.15f;
        for (int i = 0; i < powerUPCount; i++)
            status.CurrentAtackTime -= status.GetAtackTime * 0.05f;

        status.MaxHP = status.CurrentHP;
    }

    //破壊されたときにリストから外す
    void OnDisable()
    {
        if (!IsDead)
            SolgierDataBase.getInstance().RemoveList(this.gameObject);
    }

}
