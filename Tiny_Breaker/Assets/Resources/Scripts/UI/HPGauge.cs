using UnityEngine;

public class HPGauge : MonoBehaviour
{

    [SerializeField]
    Unit unit;

    //体力になる画像
    [SerializeField]
    RectTransform HP = new RectTransform();

    //体力になる画像
    [SerializeField]
    RectTransform HPBase = new RectTransform();

    void Start()
    {
        if (unit == null)
        {
            Debug.Log(this.ToString() + " Unit Nothing");
            unit = new Unit();
        }
    }

    void Update()
    {
        float HPRate = (float)unit.status.CurrentHP / (float)unit.status.MaxHP;
        //体力部分
        HP.localScale = new Vector3( HPRate, HP.localScale.y, HP.localScale.z);
        //下地部分
        HPBase.localScale = new Vector3(1.0f - HPRate, HPBase.localScale.y, HPBase.localScale.z);
    }
}
