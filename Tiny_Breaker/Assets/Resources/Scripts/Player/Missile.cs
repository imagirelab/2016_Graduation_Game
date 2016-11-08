using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour
{
    GameObject target;
    public GameObject Target { set { target = value; } }

    [SerializeField]
    float speed = 1.0f;
    [SerializeField]
    int damage = 10000;

    [SerializeField]
    float hight = 200.0f;

    Coroutine cor;
    
    IEnumerator MissileMove()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        
        Vector3 startTopPosition = transform.position + new Vector3(0.0f, hight, 0.0f);
        Vector3 targetTopPosition = target.transform.position + new Vector3(0.0f, hight, 0.0f);
        
        while (Vector3.Distance(startTopPosition, transform.position) > 10.0f)
        {
            Vector3 subVec = (startTopPosition - transform.position).normalized;

            speed *= 1.02f;

            rigidbody.velocity = subVec * speed;

            yield return null;
        }
        
        while (Vector3.Distance(targetTopPosition, transform.position) > 10.0f)
        {
            Vector3 subVec = (targetTopPosition - transform.position).normalized;

            rigidbody.velocity = subVec * speed;

            yield return null;
        }

        while (true)
        {
            Vector3 subVec = (target.transform.position - transform.position).normalized;

            rigidbody.velocity = subVec * speed;

            yield return null;
        }
    }
    
    void OnTriggerEnter(Collider collider)
    {
        //衝突時の処理
        if(collider.gameObject == target)
        {
            if (target.GetComponent<DefenseBase>())
                target.GetComponent<DefenseBase>().HPpro -= damage;
            
            Destroy(gameObject);
        }
    }
    
    void OnEnable()
    {
        cor = StartCoroutine(MissileMove());
    }

    void OnDisable()
    {
        StopCoroutine(cor);
    }
}