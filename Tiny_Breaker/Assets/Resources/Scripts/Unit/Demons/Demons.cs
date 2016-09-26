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

        //攻撃に関する設定
        GetComponent<UnitAttack>().AtkRange = ATKRange;
        GetComponent<UnitAttack>().AtkTime = status.CurrentAtackTime;
    }

    void Update()
    {
        //一番近くの敵を狙う
        SetNearTargetObject();

        //状態の切り替え
        if (GetComponent<UnitSeach>().IsFind)
            state = State.Find;
        else
            state = State.Search;

        if(GetComponent<UnitAttack>().IsAttack)
            state = State.Attack;
        
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

        Instantiate(deadEffect, this.gameObject.transform.position, deadEffect.transform.rotation);
        Instantiate(deadSE);

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
