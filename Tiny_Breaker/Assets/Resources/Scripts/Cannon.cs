using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

    public int ATK = 100;
    [SerializeField, TooltipAttribute("弾速")]
    public int SPEED = 1;

    // Use this for initialization
    void Start () {
        Vector3 castlePosition = GameObject.Find("Castle").transform.position;
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        Vector3 nearUnitPosition = units[0].transform.position;
        foreach (var e in units)
            if (Vector3.Distance(castlePosition, e.transform.position) < nearUnitPosition.magnitude)
                nearUnitPosition = e.transform.position;

        //角度計算
        Vector3 moveDirection = (nearUnitPosition - transform.position).normalized;
        //目的地への方向を見る
        transform.LookAt(transform.position + new Vector3(nearUnitPosition.x, 0, nearUnitPosition.z));
        //移動方向へ速度をSPEED分の与える
        this.GetComponent<Rigidbody>().velocity = moveDirection * SPEED;

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    // 移動の処理
    public void Move(Vector3 targetPosition)
    {
    }

    //オブジェクトが衝突したときの処理
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Unit")
        {
            collision.gameObject.GetComponent<Demons>().HPpro -= ATK;
            Debug.Log("Hit");
        }

        Destroy(gameObject);
    }
}
