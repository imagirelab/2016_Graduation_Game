// 敵のステータスなどを管理するクラス

using UnityEngine;
using StaticClass;
using System.Collections;

public class Soldier : Unit
{
    public int powerUPCount = 0;

    public GameObject dashParticle;
    private GameObject _dashParticle = null;

    IEnumerator ien;

    void Start()
    {
        Initialize();
    }

    //初期化
    void Initialize()
    {
        seach = GetComponent<UnitSeach>();
        attack = GetComponent<UnitAttack>();
        move = GetComponent<UnitMove>();

        sCollider = GetComponent<SphereCollider>();

        // 作られたときにリストに追加する
        SolgierDataBase.getInstance().AddList(this.gameObject, transform.gameObject.tag);

        //死亡フラグ
        IsDead = false;

        //攻撃に関する設定
        attack.AtkTime = status.CurrentAtackTime;

        ////設定がなされていなかった時の仮置き
        if (gameObject.transform.parent == null)
            loiteringPointObj = new Transform[] { goalObject.transform };
        if (goalObject == null)
            goalObject = GameObject.Find("DummyTarget");

        //一番近くの敵を狙う
        SetNearTargetObject();

        ien = SoldierLife();
        StartCoroutine(ien);
        StartCoroutine(NearTarget());
    }

    IEnumerator SoldierLife()
    {
        //コライダーオン
        sCollider.enabled = true;

        //移動開始
        move.enabled = true;

        float widthScale = (transform.localScale.x + transform.localScale.z) / 2.0f;
        float colliderScalingDiameter = sCollider.radius * widthScale;

        seach.enabled = false;

        //生きている時の処理
        while (status.CurrentHP > 0)
        {
            //一番近くの敵を狙う
            //SetNearTargetObject();

            //ダメージを受けたかの確認
            DamageCheck(status.CurrentHP);

            state = Enum.State.Search;

            //もしターゲットをロストしていた場合だけ急きょ次に狙うものを索敵する
            if (targetObject == null)
                SetNearTargetObject();

            //無駄な処理を省くための条件
            if (targetDistance - targetColliderRadius < ATKRange + colliderScalingDiameter)
            {
                if (targetDistance < colliderScalingDiameter) //重なっている時
                {
                    state = Enum.State.Attack;
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
                            state = Enum.State.Attack;
                            switch(type)
                            {
                                case Enum.Color_Type.Red:
                                    AxAttackSE.AxattackSEFlag = true;
                                    break;
                                case Enum.Color_Type.Green:
                                    ShieldAttackSE.ShieldattackSEFlag = true;
                                    break;
                                case Enum.Color_Type.Blue:
                                    GunAttackSE.GunattackSEFlag = true;
                                    break;
                                default:
                                    break;
                            }
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
                {
                    state = Enum.State.Find;
                    if(type == Enum.Color_Type.Green && _dashParticle == null)
                    {
                        _dashParticle = Instantiate(dashParticle);
                        _dashParticle.transform.position = this.gameObject.transform.position + Vector3.up * 1;
                        _dashParticle.transform.rotation = this.gameObject.transform.rotation;
                    }
                    else if(type == Enum.Color_Type.Green && _dashParticle != null)
                    {
                        _dashParticle.transform.position = this.gameObject.transform.position + Vector3.up * 1;
                        _dashParticle.transform.rotation = this.gameObject.transform.rotation;
                    }                    
                }                
            }

            if(_dashParticle != null && state != Enum.State.Find)
            {
                Destroy(_dashParticle);
            }

            yield return null;
        }

        //死亡処理
        state = Enum.State.Dead;
        yield return StartCoroutine(Dead());

        Destroy(gameObject);
    }

    IEnumerator Dead()
    {
        IsDead = true;

        if(_dashParticle != null && state == Enum.State.Dead)
        {
            Destroy(_dashParticle);
        }

        //死亡エフェクト出現
        if(this.gameObject.tag == "Player1")
        {
            Instantiate(reddeadEffect, this.gameObject.transform.position, reddeadEffect.transform.rotation);
        }
        else if(this.gameObject.tag == "Player2")
        {
            Instantiate(bluedeadEffect, this.gameObject.transform.position, bluedeadEffect.transform.rotation);
        } 
        else
        {
            Instantiate(bluedeadEffect, this.gameObject.transform.position, bluedeadEffect.transform.rotation);
        }
        SoundManager.deadSEFlag = true;

        //リストから外すタイミングを死んだ条件の中に入れる
        SolgierDataBase.getInstance().RemoveList(this.gameObject);

        //いらない子供から消していく
        if (transform.IsChildOf(transform))
            foreach (Transform child in transform)
            {
                //兵士用
                if (child.name == "Model")
                {
                    //トランスフォーム以外のコンポーネント
                    foreach (Component comp in child.GetComponents<Component>())
                        if (comp != child.GetComponent<Transform>())
                            Destroy(comp);

                    //コライダーがついているものをONにする
                    foreach (GameObject e in GetAllChildren.GetAll(child.gameObject))
                        if (e.GetComponent<Collider>())
                        {
                            e.GetComponent<Collider>().enabled = true;
                            e.AddComponent<Rigidbody>();
                        }
                }
                //Modelではなく、パーティクルでもないもの以外は消す
                else if (!child.gameObject.GetComponent<ParticleSystem>())
                    Destroy(child.gameObject);
            }

        //自分のコンポーネントの削除
        foreach (Component comp in this.GetComponents<Component>())
            if (comp != GetComponent<Transform>() && comp != GetComponent<Soldier>())
                Destroy(comp);

        yield return null;

        //死んでいる時の処理
        yield return new WaitForSeconds(deadTime);
    }

    void OnEnable()
    {
        if (ien != null)
        {
            // 作られたときにリストに追加する
            SolgierDataBase.getInstance().AddList(this.gameObject, transform.gameObject.tag);

            StartCoroutine(ien);
        }
    }

    //破壊されたときにリストから外す
    void OnDisable()
    {
        if (ien != null)
            StopCoroutine(ien);
        SolgierDataBase.getInstance().RemoveList(this.gameObject);
    }
}
