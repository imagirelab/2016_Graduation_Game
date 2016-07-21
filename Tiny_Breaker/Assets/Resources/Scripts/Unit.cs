using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

    //ステータス
    [SerializeField, TooltipAttribute("ステータス")]
    public Status status;
    
    //攻撃関連
    [HideInInspector]
    public bool IsAttack;           //攻撃中フラグ
    [HideInInspector]
    public GameObject attackTarget; //攻撃目標

    protected void Move()
    {
        //ターゲットがいない場合
        if (attackTarget == null)
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            return;
        }
        
        if (!IsAttack)
        {
            Vector3 targetPosition = attackTarget.transform.position;
            //目的地への方向を見る
            transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
            //目標と自分とのベクトルの差分
            Vector3 subVector = (targetPosition - transform.position).normalized;
            //角度計算
            Vector3 moveDirection = new Vector3(subVector.x, 0.0f, subVector.z);
            //移動方向へ速度をSPEED分の与える
            this.GetComponent<Rigidbody>().velocity = moveDirection * status.CurrentSPEED;
        }
    }
}
