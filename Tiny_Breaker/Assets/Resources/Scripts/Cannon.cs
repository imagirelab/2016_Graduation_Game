//城が打ち出す大砲の弾の処理を

using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {


    [SerializeField, TooltipAttribute("攻撃力")]
    private int ATK = 100;
    [SerializeField, TooltipAttribute("弾速")]
    private int SPEED = 1;

    //このクラス内で使う変数
    private GameObject attackTarget;        //攻撃対象(作った時に設定してほしい)

    //外から見れる変数
    public GameObject AttackTarget { set { attackTarget = value; } }

    // Use this for initialization
    void Start () {

        //仮に攻撃対象が設定されていなかったら破棄する
        if(attackTarget == null)
            Destroy(gameObject);

        //角度計算
        Vector3 moveDirection = (attackTarget.transform.position - transform.position).normalized;
        //目的地への方向を見る
        transform.LookAt(transform.position + new Vector3(attackTarget.transform.position.x, 0, attackTarget.transform.position.z));
        //移動方向へ速度をSPEED分の与える
        this.GetComponent<Rigidbody>().velocity = moveDirection * SPEED;

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //オブジェクトが衝突したときの処理
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Demons>() != null)
        {
            collision.gameObject.GetComponent<Demons>().HPpro -= ATK;
            Debug.Log("Cannon Hit");
        }
        
        Destroy(gameObject);
    }
}
