// 敵のステータスなどを管理するクラス

using UnityEngine;
using StaticClass;

public class Soldier : Unit
{
    ////このクラス内で使う変数       //HPのUI
    private ParticleSystem deadParticle;    //死亡時のパーティクル

    public Transform[] LoiteringPointObj;

    float deadcount;
    [SerializeField]
    float deadTime = 1.0f;
    
    void Start () {

        // 作られたときにリストに追加する
        SolgierDataBase.getInstance().AddList(this.gameObject);

        status.SetStutas();
        deadParticle = this.transform.FindChild("deadParticle").GetComponent<ParticleSystem>();

        deadcount = 0.0f;
    }

    //破壊されたときにリストから外す
    void OnDisable()
    {
        if (!IsDaed)
            SolgierDataBase.getInstance().RemoveList(this.gameObject);
    }

    void Update ()
    {
        //死んだときの処理
        if (status.CurrentHP <= 0)
        {
            //死後の処理
            Dead();

            deadcount += Time.deltaTime;

            if(deadcount > deadTime)
                Destroy(gameObject);

            ////パーティクルが止まったら本当の終わり
            //if (deadParticle.isStopped)
            //{
            //}
        }
        else
        {
            //攻撃対象がサーチ範囲内に入った場合の処理
            if (IsFind)
            {
                //攻撃対象の設定
                targetObject = DemonDataBase.getInstance().GetNearestObject(this.transform.position);

                //移動
                Move(targetObject);
            }
            else
            {
                Loitering(LoiteringPointObj);
            }
        }
    }

    void Dead()
    {
        if (!IsDaed)
        {
            IsDaed = true;

            //リストから外すタイミングを死んだ条件の中に入れる
            SolgierDataBase.getInstance().RemoveList(this.gameObject);

            deadParticle.Play();

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
