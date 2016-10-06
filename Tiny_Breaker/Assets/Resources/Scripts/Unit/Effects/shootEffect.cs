using UnityEngine;
using System.Collections;

public class shootEffect : MonoBehaviour
{
    public float animationTime = 0.5f;

    private UnitAttack _unitAttack;

    private Unit _parentUnit;

    private float _time;

	// Use this for initialization
	void Start ()
    {
        //if (_unitAttack != null)
        //{
        //    _unitAttack = GetComponentInParent<UnitAttack>();

        //    this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, Vector3.Distance(this.transform.position, _unitAttack.target.transform.position) / animationTime);
        //}

        _unitAttack = GetComponentInParent<UnitAttack>();
        _parentUnit = GetComponentInParent<Unit>();

        if (GetComponentInParent<UnitAttack>() != null)
        {
            if (_unitAttack.target != null)
            {
                //Vector3 vec = (_unitAttack.target.transform.position - this.transform.position).normalized * 30;
                //this.GetComponent<Rigidbody>().velocity = vec / animationTime;

                this.GetComponent<Rigidbody>().velocity = new Vector3((_unitAttack.target.transform.position.x - this.transform.position.x)
                    , 0
                    , (_unitAttack.target.transform.position.z - this.transform.position.z)) / animationTime;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        if(_time > animationTime)
        {
            _parentUnit.IsCharge = false;
            Destroy(this.gameObject);
        }
	}
}
