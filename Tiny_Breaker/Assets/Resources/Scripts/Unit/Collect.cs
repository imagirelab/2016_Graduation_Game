using UnityEngine;

public class Collect : UnitTrigger
{
    void Start()
    {
        //親にUnitClassを継承しているスクリプトを持っていたら登録する
        if (transform.parent.GetComponent<Unit>())
            parent = transform.parent.gameObject.GetComponent<Unit>();
        else
            Debug.Log("UnitTrigger: parent =" + parent);

        parent.IsAttack = false;
    }

    void Update()
    {
        //if (hitTarget != null && hitTarget == parent.targetObject)
        //{
        //    //魂を持ってるオブジェクトがターゲットの時
        //    if (hitTarget.GetComponent<Spirits>())
        //    {
        //        Transform rootTransform = hitTarget.transform.root;
        //        //成長値だけ拾う
        //        this.gameObject.transform.root.gameObject.GetComponent<Player>().AddSpiritList(hitTarget.GetComponent<Spirits>().GrowPoint);

        //        //子供から消していく
        //        if (rootTransform.IsChildOf(rootTransform))
        //            foreach (Transform child in rootTransform)
        //                Destroy(child.gameObject);
        //        Destroy(rootTransform.gameObject);
        //    }
        //}
    }
}
