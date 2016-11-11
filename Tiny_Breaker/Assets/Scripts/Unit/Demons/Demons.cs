using UnityEngine;
using StaticClass;
using System.Collections;

public class Demons : Unit
{
    [SerializeField, TooltipAttribute("種類")]
    Enum.Demon_TYPE DemonType = Enum.Demon_TYPE.POPO;

    [SerializeField, TooltipAttribute("召喚中に消すステータスのUI")]
    GameObject statusUI;

    SpawnMove spawn;

    void Start()
    {
        Initialize();
    }

    //初期化
    void Initialize()
    {
        if (statusUI == null)
            statusUI = new GameObject();

        seach = GetComponent<UnitSeach>();
        attack = GetComponent<UnitAttack>();
        move = GetComponent<UnitMove>();
        spawn = GetComponent<SpawnMove>();

        sCollider = GetComponent<SphereCollider>();

        //出現目標値を入れる
        spawn.SetTargetVec(SpawnTargetPosition - transform.position);

        //死亡フラグ
        IsDead = false;

        //攻撃に関する設定
        attack.AtkTime = status.CurrentAtackTime;
        //巡回速度
        loiteringSPEED = status.CurrentSPEED;

        ////設定がなされていなかった時の仮置き
        if (gameObject.transform.parent == null)
            loiteringPointObj = new Transform[] { goalObject.transform };
        if (goalObject == null)
            goalObject = GameObject.Find("DummyTarget");

        //一番近くの敵を狙う
        SetNearTargetObject();

        StartCoroutine(DemonLife());
        StartCoroutine(NearTarget());
    }

    IEnumerator DemonLife()
    {
        //出撃
        spawn.enabled = true;

        //ステータスUIを消す
        statusUI.SetActive(false);
        //無敵起動
        StartCoroutine(Invincible());

        //出撃完了まで待つ
        while(!spawn.End)
            yield return null;

        //出撃スクリプト停止
        spawn.enabled = false;
        //ステータスUIを表示
        statusUI.SetActive(true);
        //コライダーオン
        sCollider.enabled = true;

        //移動開始
        move.enabled = true;
        
        float colliderScalingDiameter = sCollider.radius * transform.localScale.x;

        seach.enabled = false;

        //生きている時の処理
        while (status.CurrentHP > 0)
        {
            //一番近くの敵を狙う
            //SetNearTargetObject();

            //ダメージを受けたかの確認
            DamageCheck(status.CurrentHP);

            state = State.Search;

            //無駄な処理を省くための条件
            if (targetDistance - targetColliderRadius < ATKRange + colliderScalingDiameter)
            {
                if (targetDistance < colliderScalingDiameter) //重なっている時
                {
                    state = State.Attack;
                    seach.enabled = false;
                    attack.enabled = true;
                }
                else
                {
                    RaycastHit hit;
                    Vector3 vec = targetObject.transform.position - transform.position;
                    Ray ray = new Ray(transform.position, vec);
                    ray.origin += new Vector3(0.0f, 1.5f, 0.0f);    //視線の高さ分上げている形

                    int layerMask = ~(1 << transform.gameObject.layer | 1 << 18);  //自身のレイヤー番号とGround以外にヒットするようにしたビット演算
                    if (Physics.SphereCast(ray, 1.5f, out hit, ATKRange + colliderScalingDiameter, layerMask))
                    {
                        if (hit.collider.gameObject == targetObject)
                        {
                            state = State.Attack;
                            seach.enabled = false;
                            attack.enabled = true;
                        }
                    }
                }
            }
            else if (targetDistance - targetColliderRadius <= seach.findRange + colliderScalingDiameter &&
                    targetDistance - targetColliderRadius >= ATKRange + colliderScalingDiameter)
            {
                seach.enabled = true;
                attack.enabled = false;
            }
            else
            {
                attack.enabled = false;
            }

            //発見状態の条件
            if (seach.enabled)
            {
                if (seach.IsLose)
                    seach.enabled = false;
                if (seach.IsFind)
                    state = State.Find;
            }

            yield return null;
        }

        //死亡処理
        state = State.Dead;
        yield return StartCoroutine(Dead());

        Destroy(gameObject);
    }
    
    IEnumerator Dead()
    {
        IsDead = true;

        //リストから外す
        DemonDataBase.getInstance().RemoveList(this.gameObject);

        //死んだ直後に魂を回収してみる
        if (transform.parent != null)
            transform.parent.gameObject.GetComponent<Player>().AddSpiritList(DemonType);

        //死亡エフェクト
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

        yield return null;

        //死んでいる時の処理
        while (deadcount <= deadTime)
        {
            deadcount += Time.deltaTime;

            GetComponent<Rigidbody>().velocity = transform.forward * -1 * deadMoveSpeed;

            yield return null;
        }
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

    //破壊されたときにリストから外す
    void OnDisable()
    {
        DemonDataBase.getInstance().RemoveList(this.gameObject);
    }
}