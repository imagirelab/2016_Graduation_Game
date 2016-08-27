using UnityEngine;

public class SeachRange : UnitTrigger
{
    [SerializeField, TooltipAttribute("見失うまでの時間")]
    float loseTime = 3.0f;  //見失うまでの時間
    float loseCounter = 0.0f;   //見失っている時間
    
    void Start()
    {
        //親にUnitClassを継承しているスクリプトを持っていたら登録する
        if (transform.parent.GetComponent<Unit>())
            parent = transform.parent.GetComponent<Unit>();
        else
        {
            parent = new Unit();
            Debug.Log("UnitTrigger: parent =" + parent);
        }

        parent.IsFind = false;
        loseCounter = 0.0f;
    }

    void Update()
    {
        //if (!(/*hitTarget != null && */parent.targetObject != null))
        //{
        //    //hitTarget = null;
        //    hitFlag = false;
        //}
        
        //見失う時間の扱い
        if (hitFlag)
        {
            loseCounter = 0.0f;
            parent.IsFind = true;
        }
        else
            loseCounter += Time.deltaTime;
        
        //発見フラグが戻る条件
        if (loseCounter > loseTime || hitTarget == null)
            parent.IsFind = false;
    }
}
