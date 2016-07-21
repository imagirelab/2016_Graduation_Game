//お城からの攻撃タイミングの処理をするクラス

using UnityEngine;

public class CastleAttack : MonoBehaviour {
    
    [SerializeField, TooltipAttribute("攻撃間隔")]
    private float attackTime = 1.0f;                //攻撃間隔
    [SerializeField, TooltipAttribute("大砲の発射位置(調整数値)")]
    private Vector3 cannonPosition = Vector3.zero;  //大砲の発射位置(調整数値)

    //このクラス内で使う変数
    private float time;                 //時間
    private bool IsAttack;              //攻撃中フラグ
    private GameObject attackTarget;    //攻撃対象

    //外から見れる変数
    public GameObject AttackTarget { get { return attackTarget; } }     //attackTargetのプロパティ


    // Use this for initialization
    void Start () {

        time = 0.0f;
        IsAttack = false;
    }
	
	// Update is called once per frame
	void Update () {

        //アタックタイムを満たしていて攻撃フラグが立っていたら攻撃
        if (time > attackTime && IsAttack)
        {
            time = 0;

            //攻撃対象がいることを確認してから攻撃
            if (attackTarget != null)
            {
                GameObject cannonPrefab = (GameObject)Resources.Load("Prefabs/Cannon");
                GameObject cannon = (GameObject)Instantiate(cannonPrefab,
                                                            transform.position + cannonPosition,
                                                            Quaternion.identity);
                //攻撃対象を大砲の弾に渡す
                cannon.gameObject.GetComponent<Cannon>().AttackTarget = attackTarget;
            }
            else
                IsAttack = false;
        }

        //1フレームあたりの時間を取得
        time += Time.deltaTime;
    }

    void OnTriggerEnter(Collider collider)
    {
        //悪魔が範囲内に入ってきたとき攻撃対象が登録されていなければ登録する
        if (collider.gameObject.GetComponent<Demons>() != null &&
            attackTarget == null)
        {
            IsAttack = true;
            attackTarget = collider.gameObject;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        //まだ触れているのに攻撃対象がいなかった場合別のターゲットが範囲内に入っていることになるので
        //そちらのほうに攻撃対象を移す
        if (collider.gameObject.GetComponent<Demons>() != null)
        {
            if (attackTarget == null && collider.gameObject != null)
            {
                IsAttack = true;
                attackTarget = collider.gameObject;
            }
        }
        //悪魔以外が範囲内に入っていたら攻撃をやめる
        else
        {
            attackTarget = null;
            IsAttack = false;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        //範囲内から出たら攻撃対象から外す
        if (attackTarget == collider.gameObject)
        {
            attackTarget = null;
            IsAttack = false;
        }
    }
}
