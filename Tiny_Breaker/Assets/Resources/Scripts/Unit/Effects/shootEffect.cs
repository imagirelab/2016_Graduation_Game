using UnityEngine;
using System.Collections;

public class shootEffect : MonoBehaviour
{
    public float animationTime = 0.5f;

    private UnitAttack _unitAttack;

    private float _time;

	// Use this for initialization
	void Start ()
    {
        //if (_unitAttack != null)
        //{
        //    _unitAttack = GetComponentInParent<UnitAttack>();

        //    this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, Vector3.Distance(this.transform.position, _unitAttack.target.transform.position) / animationTime);
        //}
	}

    // Update is called once per frame
    void Update()
    {
        _unitAttack = GetComponentInParent<UnitAttack>();

        if (GetComponentInParent<UnitAttack>() != null)
        {
            if (_unitAttack.target != null)
            {
                Vector3 vec = (_unitAttack.target.transform.position - this.transform.position).normalized * 30;
                this.GetComponent<Rigidbody>().velocity = vec / animationTime;
            }
        }

        _time += Time.deltaTime;

        if(_time > animationTime)
        {
            Destroy(this.gameObject);
        }
	}
}
