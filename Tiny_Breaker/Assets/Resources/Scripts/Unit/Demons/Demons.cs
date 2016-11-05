using UnityEngine;
using StaticClass;
using System.Collections;

public class Demons : Unit
{
    [SerializeField, TooltipAttribute("種類")]
    Enum.Demon_TYPE DemonType = Enum.Demon_TYPE.POPO;
    
    void Start()
    {
        Initialize();
    }

    //初期化
    void Initialize()
    {
        //無敵起動
        StartCoroutine(Invincible());

        //死亡フラグ
        IsDead = false;
        
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

        loiteringSPEED = status.CurrentSPEED;
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

            //リストから外す
            DemonDataBase.getInstance().RemoveList(this.gameObject);

            //死んだ直後に魂を回収してみる
            if (transform.parent != null)
                transform.parent.gameObject.GetComponent<Player>().AddSpiritList(DemonType);

            Instantiate(deadEffect, this.gameObject.transform.position, deadEffect.transform.rotation);
            SoundManager.deadSEFlag = true;

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
    }
    
    //破壊されたときにリストから外す
    void OnDisable()
    {
        DemonDataBase.getInstance().RemoveList(this.gameObject);
    }

    //無敵の処理（無敵になるタイミングで呼び出す）
    IEnumerator Invincible()
    {
        // リストから外す
        DemonDataBase.getInstance().RemoveList(this.gameObject);
        
        yield return new WaitForSeconds(invincibleTime);
        
        // リストに追加する
        DemonDataBase.getInstance().AddList(this.gameObject);

        yield return null;
    }
}