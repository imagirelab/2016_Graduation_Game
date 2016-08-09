using UnityEngine;

public class SeachRange : UnitTrigger
{
    void Start()
    {
        //親にUnitClassを継承しているスクリプトを持っていたら登録する
        if (transform.parent.GetComponent<Unit>())
            parent = transform.parent.GetComponent<Unit>();
        else
            Debug.Log("UnitTrigger: parent =" + parent);

        parent.IsFind = false;
    }

    void Update()
    {
        if (!(hitTarget != null && parent.targetObject != null))
        {
            hitTarget = null;
            hitFlag = false;
        }

        //発見フラグの更新
        parent.IsFind = hitFlag;
    }
}
