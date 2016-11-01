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
        //はじめ無敵スタート
        invincibleFlag = true;
        
        //死亡フラグ
        IsDead = false;

        //ステータスの決定
        SetStatus();

        //攻撃に関する設定
        GetComponent<UnitAttack>().AtkRange = ATKRange;
        GetComponent<UnitAttack>().AtkTime = status.CurrentAtackTime;

        //////設定がなされていなかった時の仮置き
        if (goalObject == null)
            goalObject = GameObject.Find("DummyTarget");
        //一番近くの敵を狙う
        SetNearTargetObject();

        //巡回ルート
        if (gameObject.transform.parent == null)
            loiteringPointObj = new Transform[] { goalObject.transform };
    }

    void Update()
    {
        //無敵
        if (invincibleFlag)
            Invincible();

        if (IsDead)
        {
            state = State.Dead;
            Dying();
        }
        else
        {
            //死亡処理
            if (status.CurrentHP <= 0)
                Dead();

            //一番近くの敵を狙う
            SetNearTargetObject();

            //状態の切り替え
            if (GetComponent<UnitSeach>().IsFind)
                state = State.Find;
            else
                state = State.Search;

            if (GetComponent<UnitAttack>().IsAttack)
                state = State.Attack;

            //ダメージを受けたかの確認
            DamageCheck(status.CurrentHP);
        }
    }

    //死んでいる時の処理
    void Dying()
    {
        deadcount += Time.deltaTime;

        GetComponent<Rigidbody>().velocity = transform.forward * -1 * deadMoveSpeed;

        if (deadcount > deadTime)
            Destroy(gameObject);
    }

    //死んだときの処理
    void Dead()
    {
        if (!IsDead)
        {
            IsDead = true;

            DemonDataBase.getInstance().RemoveList(this.gameObject);

            //死んだ直後に魂を回収してみる
            if (transform.parent != null)
                transform.parent.gameObject.GetComponent<Player>().AddSpiritList(growPoint);

            Instantiate(deadEffect, this.gameObject.transform.position, deadEffect.transform.rotation);
            Instantiate(deadSE);

            //いらない子供から消していく
            if (transform.IsChildOf(transform))
                foreach (Transform child in transform)
                {
                    //Modelsの中の削除処理
                    if (child.name == "Models")
                    {
                        //トランスフォーム以外のコンポーネント
                        foreach (Component comp in child.GetComponents<Component>())
                            if (comp != child.GetComponent<Transform>())
                                Destroy(comp);
                    }
                    else
                        Destroy(child.gameObject);
                }

            //自分のコンポーネントの削除
            foreach (Component comp in this.GetComponents<Component>())
                if (comp != GetComponent<Transform>() && comp != GetComponent<Demons>() && comp != GetComponent<Rigidbody>())
                    Destroy(comp);
        }
        //Destroy(gameObject);
    }

    //ステータスの設定
    public void SetStatus()
    {
        //基礎ステータスの代入
        status.SetStatus();

        //今のステータスを算出する
        float hp = status.GetHP;
        float atk = status.GetATK;

        for (int i = 0; i < growPoint.CurrentHP_GrowPoint - growPoint.GetHP_GrowPoint; i++)
            hp *= 1.1f;
        for (int i = 0; i < growPoint.CurrentATK_GrowPoint - growPoint.GetATK_GrowPoint; i++)
            atk *= 1.1f;
        //status.CurrentHP += (int)(status.GetHP * 0.1f);
        //status.CurrentATK += (int)(status.GetATK * 0.1f);
        //for (int i = 0; i < growPoint.CurrentSPEED_GrowPoint - growPoint.GetSPEED_GrowPoint; i++)
        //    status.CurrentSPEED += status.GetSPEED * 0.15f;
        //for (int i = 0; i < growPoint.CurrentAtackTime_GrowPoint - growPoint.GetAtackTime_GrowPoint; i++)
        //    status.CurrentAtackTime -= status.GetAtackTime * 0.05f;

        status.CurrentHP = (int)hp;
        status.CurrentATK = (int)atk;

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
        if (DemonDataBase.getInstance().ChackKey(this.gameObject))
            DemonDataBase.getInstance().RemoveList(this.gameObject);
    }

    //無敵の処理
    void Invincible()
    {
        if (DemonDataBase.getInstance().ChackKey(this.gameObject))
            DemonDataBase.getInstance().RemoveList(this.gameObject);

        invincibleCount += Time.deltaTime;

        if (invincibleCount >= invincibleTime)
        {
            invincibleCount = 0.0f;
            // 作られたときにリストに追加する
            DemonDataBase.getInstance().AddList(this.gameObject, transform.gameObject.tag);
        }
    }
}
