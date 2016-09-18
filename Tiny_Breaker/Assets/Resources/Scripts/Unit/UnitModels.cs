using UnityEngine;
using System;

public class UnitModels : MonoBehaviour
{

    //Unitコンポーネント
    Unit unit;
    //子の数
    int childCount = 0;

    void Start()
    {
        //
        GameObject parent = gameObject;
        if (gameObject.transform.parent != null)
            parent = gameObject.transform.parent.gameObject;

        if (parent.GetComponent<Unit>() != null)
            unit = parent.GetComponent<Unit>();
        else
            unit = null;

        //子の数を取得
        childCount = transform.childCount;
    }

    void Update()
    {
        float parHPchild = 1.0f / childCount; //子供分のHPのパーセンテージ
        float parHPnow = (float)unit.status.CurrentHP / (float)unit.status.MaxHP; //現在のHPのパーセンテージ

        int childHPcount = (int)Math.Ceiling((double)(parHPnow / parHPchild)); //現在の体力の割合から何体分の体力が残っているか

        //現在の子供の数が残るべき数より多かったら減らす処理
        if (transform.childCount > childHPcount)
        {
            int subChildCount = transform.childCount - childHPcount;
            int count = 0;

            foreach (Transform child in transform)
            {
                count++;
                if (count > subChildCount)
                    return;

                //一番最後の一匹になるまではこちらで消す
                if (transform.childCount > 1)
                    Destroy(child.gameObject);
            }
        }

    }
}
