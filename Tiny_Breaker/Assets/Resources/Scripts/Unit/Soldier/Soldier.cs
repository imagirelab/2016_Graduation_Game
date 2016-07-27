// 敵のステータスなどを管理するクラス

using UnityEngine;
using StaticClass;

public class Soldier : Unit
{
    ////このクラス内で使う変数       //HPのUI
    private ParticleSystem deadParticle;    //死亡時のパーティクル

    public Transform[] LoiteringPointObj;
    
    void Start () {

        // 作られたときにリストに追加する
        SolgierDataBase.getInstance().AddList(this.gameObject);

        status.SetStutas();
        deadParticle = this.transform.FindChild("deadParticle").GetComponent<ParticleSystem>();
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

            //パーティクルが止まったら本当の終わり
            if (deadParticle.isStopped)
            {
                Destroy(gameObject);
            }
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
                    if (!child.GetComponent<ParticleSystem>())   //パーティクルシステム以外を消す
                        Destroy(child.gameObject);
        }
    }
}
