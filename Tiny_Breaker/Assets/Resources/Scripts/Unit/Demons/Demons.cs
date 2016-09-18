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
        if (targetObject != null)
        {
            //発見
            IsFind = false;
            if (Vector3.Distance(transform.position, targetObject.transform.position) < 40.0f)  //40.0f = Collider.Radius * LocalScale
            {
                RaycastHit hit;
                Vector3 vec = targetObject.transform.position - transform.position;
                Ray ray = new Ray(transform.position, vec);
                if (Physics.Raycast(ray, out hit, 40.0f))
                {
                    if(hit.collider.gameObject == targetObject)
                    {
                        IsFind = true;
                        state = State.Find;
                    }
                }
            }

            //攻撃
            IsAttack = false;
            if (Vector3.Distance(transform.position, targetObject.transform.position) < ATKRange * transform.localScale.x + 1.0f)  //攻撃範囲
            {
                RaycastHit hit;
                Vector3 vec = targetObject.transform.position - transform.position;
                Ray ray = new Ray(transform.position, vec);
                if (Physics.Raycast(ray, out hit, ATKRange * transform.localScale.x + 1.0f))
                {
                    if (hit.collider.gameObject == targetObject)
                    {
                        IsAttack = true;
                        gameObject.GetComponent<UnitAttack>().enabled = true;
                        state = State.Attack;
                    }
                }
            }

            //徘徊
            if(!IsFind && !IsAttack)
            {
                gameObject.GetComponent<UnitAttack>().enabled = false;
                state = State.Search;
            }
        }
        else
        {
            IsFind = false;
            IsAttack = false;
            gameObject.GetComponent<UnitAttack>().enabled = false;
            state = State.Wait;
        }
        
        //死亡処理
        if (status.CurrentHP <= 0)
        Dead();
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
            status.CurrentHP += (int)(status.GetHP * 0.1f);
        for (int i = 0; i < growPoint.CurrentATK_GrowPoint - growPoint.GetATK_GrowPoint; i++)
            status.CurrentATK += (int)(status.GetATK * 0.1f);
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
