// 悪魔単体の処理をするクラス

using UnityEngine;
using System.Collections;

public class Demons : MonoBehaviour
{
    //プレイヤーの仮ステータス
    public int defaultHP = 100;
    public int defaultHPpro { get { return defaultHP; }}
    public int HP = 100;
    public int HPpro { get { return HP; } set { HP = value; } }
    public int defaultATK = 100;
    public int defaultATKpro { get { return defaultATK; }}
    public int ATK = 100;
    public int ATKpro { get { return ATK; } set { ATK = value; } }
    public int defaultSPEED = 5;
    public int defaultSPEEDpro { get { return defaultSPEED; } }
    public int SPEED = 5;
    public int SPEEDpro { get { return SPEED; } set { SPEED = value; } }
    public float AtackTime = 1.0f;
    public float ATKTimepro { get { return AtackTime; } }

    public GameObject demonSpirit;

    private Vector3 moveDirection;      //移動する方向の角度
    private float time;                 //時間
    //private bool moveFlag;              //オブジェクトごとの移動フラグ

    // 接触しているゲームオブジェクト
    private GameObject hitCollisionObject;

    // Use this for initialization
    void Start ()
    {
        //moveFlag = false;
        moveDirection = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //死亡処理
        if (HP <= 0)
        {
            demonSpirit.GetComponent<DemonSpirits>().HPpro = 100;
            demonSpirit.GetComponent<DemonSpirits>().ATKpro = 10;
            demonSpirit.GetComponent<DemonSpirits>().SPEEDpro = 1;

            //スピリットの生成
            Instantiate(demonSpirit, this.gameObject.transform.position, this.gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }

    // 移動命令の処理
    public void MoveOrder(Vector3 targetPosition)
    {
        if (this.gameObject)
        {
            //目的地と0.1mより離れている場合の処理
            if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                //角度計算
                moveDirection = (targetPosition - transform.position).normalized;
                //目的地への方向を見る
                transform.LookAt(transform.position + new Vector3(targetPosition.x, 0, targetPosition.z));
                //移動方向へ速度をSPEED分の与える
                this.GetComponent<Rigidbody>().velocity = moveDirection * SPEED;
            }
            //目的地に0.1mより近い距離になった場合の処理
            else
            {
                //速度を０に
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }

    // 建造物に向かい攻撃する命令の処理
    public void BuildingOrder(GameObject target)
    {
        //ターゲットが何も無ければ処理しない
        if (target == null)
            return;

        //目的地と離れている場合の処理
        if (hitCollisionObject != target)
        {
            //角度計算
            moveDirection = (target.transform.position - transform.position).normalized;
            //目的地への方向を見る
            transform.LookAt(transform.position + new Vector3(target.transform.position.x, 0, target.transform.position.z));
            //移動方向へ速度をSPEED分の与える
            this.GetComponent<Rigidbody>().velocity = moveDirection * SPEED;
        }
        //目的地に触れている場合の処理
        else
        {
            //速度を０に
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;

            //アタックタイムを満たしたら
            if (time > AtackTime)
            {
                time = 0;

                // お城クラスを持っていたら処理
                if(target.GetComponent<Castle>() != null)
                    target.GetComponent<Castle>().HPpro -= ATK;
            }

            //1フレームあたりの時間を取得
            time += Time.deltaTime;

        }
    }

    // 敵に攻撃する命令の処理
    public void EnemyOrder()
    {
        //今はとりあえず動きを止める
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    //待機命令の処理(いらないかもしれないが)
    public void WaitOrder()
    {
        //今はとりあえず動きを止める
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    //オブジェクトが衝突したときの処理
    void OnCollisionEnter(Collision collision)
    {
        //接触したゲームオブジェクトを保存
        hitCollisionObject = collision.gameObject;
    }

    //オブジェクトが触れている間
    void OnCollisionStay(Collision collision)
    {
        //接触しているゲームオブジェクトを保存
        hitCollisionObject = collision.gameObject;
    }

    //オブジェクトが離れた時
    void OnCollisionExit(Collision collision)
    {
        //離れたら戻す
        hitCollisionObject = null;
    }    
}
