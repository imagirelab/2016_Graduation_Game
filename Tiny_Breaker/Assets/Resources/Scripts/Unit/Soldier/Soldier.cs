// 敵のステータスなどを管理するクラス

using UnityEngine;
using StaticClass;

public class Soldier : Unit
{
    public int powerUPCount = 0;
    
    void Start()
    {
        // 作られたときにリストに追加する
        SolgierDataBase.getInstance().AddList(this.gameObject, transform.gameObject.tag);
        
        deadcount = 0.0f;

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

        if (deadcount > deadTime)
            Destroy(gameObject);
    }

    //死んだときの処理
    void Dead()
    {
        if (!IsDead)
        {
            IsDead = true;

            //死亡エフェクト出現
            Instantiate(deadEffect, this.gameObject.transform.position, deadEffect.transform.rotation);
            SoundManager.deadSEFlag = true;

            //リストから外すタイミングを死んだ条件の中に入れる
            SolgierDataBase.getInstance().RemoveList(this.gameObject);

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

                        //孫の削除処理
                        foreach (Transform grandson in child)
                        {
                            //兵士用
                            if (grandson.name == "Model")
                            {
                                //トランスフォーム以外のコンポーネント
                                foreach (Component comp in grandson.GetComponents<Component>())
                                    if (comp != grandson.GetComponent<Transform>())
                                        Destroy(comp);

                                //コライダーがついているものをONにする
                                foreach (GameObject e in GetAllChildren.GetAll(grandson.gameObject))
                                    if (e.GetComponent<Collider>())
                                    {
                                        e.GetComponent<Collider>().enabled = true;
                                        e.AddComponent<Rigidbody>();
                                    }
                            }
                            //Modelではなく、パーティクルでもないもの以外は消す
                            else if(!grandson.gameObject.GetComponent<ParticleSystem>())
                                Destroy(grandson.gameObject);
                        }
                      
                    }
                    else
                        Destroy(child.gameObject);
                }

            //自分のコンポーネントの削除
            foreach (Component comp in this.GetComponents<Component>())
                if (comp != GetComponent<Transform>() && comp != GetComponent<Soldier>())
                    Destroy(comp);
        }
    }

    //ステータスの設定
    void SetStatus()
    {
        //基礎ステータスの代入
        status.SetStatus(0);

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
