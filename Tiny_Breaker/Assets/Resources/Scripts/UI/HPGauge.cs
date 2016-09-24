using UnityEngine;
using UnityEngine.UI;

public class HPGauge : MonoBehaviour
{

    [SerializeField]
    Unit unit;

    Slider hpGauge;

    void Start()
    {
        if (unit == null)
        {
            Debug.Log(this.ToString() + " Unit Nothing");
            unit = new Unit();
        }

        if (GetComponent<Slider>() != null)
            hpGauge = GetComponent<Slider>();
    }

    void Update()
    {
        float HPRate = (float)unit.status.CurrentHP / (float)unit.status.MaxHP;
        hpGauge.value = HPRate * 100;
    }
}
