using UnityEngine;
using System.Collections;
using StaticClass;

public class SeachRange : UnitTrigger
{

	// Use this for initialization
	void Start ()
    {
        //親にUnitClassを継承しているスクリプトを持っていたら登録する
        if (transform.parent.GetComponent<Unit>())
            parent = transform.parent.GetComponent<Unit>();
        else
            Debug.Log("UnitTrigger: parent =" + parent);

        parent.IsFind = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        //悪魔が範囲内に入ってきたときに向かう
        if (collider.gameObject.tag == "Player/Unit")
        {
            if (!parent.IsDaed)
            {
                parent.IsFind = true;
            }
            else
            {
                parent.IsFind = false;
            }
        }
    }

    void OnTriggerStay(Collider collider)
    {
        //悪魔が範囲内に入っているとき向かい続ける
        if (collider.gameObject.tag == "Player/Unit")
        {
            if (!parent.IsDaed)
            {
                parent.IsFind = true;
            }
            else
            {
                parent.IsFind = false;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        //範囲内から出たら向かうのをやめて巡回する
        if (collider.gameObject.tag == "Player/Unit")
        {
            parent.IsFind = false;
            hitTarget = null;
        }
    }
}
