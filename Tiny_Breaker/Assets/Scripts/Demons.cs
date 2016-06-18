using UnityEngine;
using System.Collections;

public class Demons : MonoBehaviour
{
    //プレイヤーの仮ステータス
    public int HP = 1000;
    public int ATK = 100;
    public int DEF = 100;
    public int SPEED = 100;

    private Vector3 moveDirection;      //移動する方向の角度
    private bool moveFlag;              //オブジェクトごとの移動フラグ

    // Use this for initialization
    void Start ()
    {        
        moveFlag = false;
        moveDirection = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //座標指示されたら移動フラグを成立
	    if(StaticVariables.goFlag)
        {
            moveFlag = true;
        }

        if (moveFlag)
        {
            //目的地と0.1mより離れている場合の処理
            if (Vector3.Distance(transform.position, StaticVariables.goPosition) > 0.1f)
            {
                //角度計算
                moveDirection = (StaticVariables.goPosition - transform.position).normalized;
                //目的地への方向を見る
                transform.LookAt(transform.position + new Vector3(StaticVariables.goPosition.x, 0, StaticVariables.goPosition.z));
                //移動方向へ速度をSPEED分の与える
                this.GetComponent<Rigidbody>().velocity = moveDirection * SPEED;
            }
            //目的地に0.1mより近い距離になった場合の処理
            else
            {
                //移動フラグをオフに
                moveFlag = false;
                //指示フラグもオフに
                StaticVariables.goFlag = false;
                //速度を０に
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }
}
