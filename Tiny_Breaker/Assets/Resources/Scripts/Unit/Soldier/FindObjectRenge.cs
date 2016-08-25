using UnityEngine;
using System.Collections;

public class FindObjectRenge : MonoBehaviour
{
    //Unitクラスの親
    Unit parent;

    void Start()
    {
        //親にUnitClassを継承しているスクリプトを持っていたら登録する
        if (transform.parent.GetComponent<Unit>())
            parent = transform.parent.gameObject.GetComponent<Unit>();
        else
            Debug.Log("UnitTrigger: parent =" + parent);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<DefenseBase>() != null)
            parent.IsDefenseBase = true;
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.GetComponent<DefenseBase>() != null)
            parent.IsDefenseBase = true;
    }

    void OnTriggerExit(Collider collider)
    {
        parent.IsDefenseBase = false;
    }
}
