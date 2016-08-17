// 敵のステータスなどを管理するクラス

using UnityEngine;
using StaticClass;

public class Soldier : Unit
{
    ////このクラス内で使う変数       //HPのUI
    //private ParticleSystem deadParticle;    //死亡時のパーティクル

    public Transform[] LoiteringPointObj;

    float deadcount;
    [SerializeField]
    float deadTime = 1.0f;

    AudioSource se;
    bool onecall = false;
    
    void Start () {

        // 作られたときにリストに追加する
        SolgierDataBase.getInstance().AddList(this.gameObject);

        //ステータスの設定
        status.SetStatus();
        status.MaxHP = status.CurrentHP;

        //deadParticle = this.transform.FindChild("deadParticle").GetComponent<ParticleSystem>();

        deadcount = 0.0f;

        if (se == null)
            gameObject.AddComponent<AudioSource>();
        se = GetComponent<AudioSource>();
    }

    //破壊されたときにリストから外す
    void OnDisable()
    {
        if (!IsDead)
            SolgierDataBase.getInstance().RemoveList(this.gameObject);
    }

    void Update ()
    {
        //常に近くの悪魔を攻撃対象に設定
        targetObject = DemonDataBase.getInstance().GetNearestObject(this.transform.position);

        //死んだときの処理
        if (status.CurrentHP <= 0)
        {
            //死後の処理
            Dead();

            deadcount += Time.deltaTime;

            if(deadcount > deadTime)
                Destroy(gameObject);
        }
        else
        {
            //攻撃対象がサーチ範囲内に入った場合の処理
            if (IsFind)
            {

                if (!onecall)
                {
                    onecall = true;
                    se.Play();
                }
                //移動
                Move(targetObject);
            }
            else
            {
                onecall = false;
                Loitering(LoiteringPointObj);
            }
        }
    }

    //死んだときの処理
    void Dead()
    {
        if (!IsDead)
        {
            IsDead = true;

            //リストから外すタイミングを死んだ条件の中に入れる
            SolgierDataBase.getInstance().RemoveList(this.gameObject);

            //deadParticle.Play();

            //いらない子供から消していく
            if (transform.IsChildOf(transform))
                foreach (Transform child in transform)
                    if (child.name == "Model")
                    {
                        foreach (GameObject e in GetAllChildren.GetAll(child.gameObject))
                        {
                            if (e.GetComponent<Collider>())
                            {
                                e.GetComponent<Collider>().enabled = true;
                                e.AddComponent<Rigidbody>();
                            }
                        }
                    }
                    else
                        Destroy(child.gameObject);

            foreach (Component comp in this.GetComponents<Component>())
                if (comp != GetComponent<Transform>() && comp != GetComponent<Soldier>())
                    Destroy(comp);
        }
    }
}
