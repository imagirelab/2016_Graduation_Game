using UnityEngine;

public class shootEffect : MonoBehaviour
{
    //ショットの速さ
    [SerializeField]
    float shotSpeed = 1.0f;

    GameObject target;
    Vector3 offset = Vector3.zero;
    public Vector3 Offset { set { offset = value; } }

    void Start()
    {
        UnitAttack _unitAttack = GetComponentInParent<UnitAttack>();

        if (GetComponentInParent<UnitAttack>() != null)
        {
            if (_unitAttack.Target != null)
            {
                target = _unitAttack.Target;

                this.GetComponent<Rigidbody>().velocity = new Vector3(target.transform.position.x - this.transform.position.x,
                    target.transform.position.y + offset.y - this.transform.position.y,
                    target.transform.position.z - this.transform.position.z);
                this.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity.normalized * shotSpeed;
            }
        }
    }

    void Update()
    {
        //ホーミングして必ず当たる
        if (target != null)
        {
            //ある程度近くても消える
            if (Vector3.Distance(this.transform.position, target.transform.position) < 10.0f)
                Destroy(this.gameObject);

            this.GetComponent<Rigidbody>().velocity = new Vector3(target.transform.position.x - this.transform.position.x,
                target.transform.position.y + offset.y - this.transform.position.y,
                target.transform.position.z - this.transform.position.z);
            this.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity.normalized * shotSpeed;
        }
        else
            Destroy(this.gameObject);

        //死んでいたら消える
        if (GetComponentInParent<Unit>().state == Unit.State.Dead)
            Destroy(this.gameObject);
    }

    //ぶつかったら消える
    void OnTriggerEnter(Collider collider)
    {
        if (target == collider.gameObject)
            Destroy(this.gameObject);
    }
}
