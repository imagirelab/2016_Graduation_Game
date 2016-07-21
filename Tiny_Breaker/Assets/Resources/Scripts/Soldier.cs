// 敵のステータスなどを管理するクラス

using UnityEngine;
using StaticClass;

public class Soldier : Unit
{
    ////このクラス内で使う変数       //HPのUI
    private ParticleSystem deadParticle;    //死亡時のパーティクル
    private bool deadFlag = false;      //死亡判定
    
    void Start () {

        // 作られたときにリストに追加する
        SolgierDataBase.getInstance().AddList(this.gameObject);

        status.SetStutas();
        deadParticle = this.transform.FindChild("deadParticle").GetComponent<ParticleSystem>();
    }

    //破壊されたときにリストから外す
    void OnDisable()
    {
        SolgierDataBase.getInstance().RemoveList(this.gameObject);
    }
    
    void Update ()
    {
        //攻撃対象の設定
        attackTarget = DemonDataBase.getInstance().GetNearestObject(this.transform.position);

        //移動
        Move();

        if (status.CurrentHP <= 0)
        {
            if (!deadFlag)
            {
                deadParticle.Play();
                deadFlag = true;
            }

            if (deadParticle.isStopped)
            {
                Destroy(gameObject);
            }
        }
    }
}
