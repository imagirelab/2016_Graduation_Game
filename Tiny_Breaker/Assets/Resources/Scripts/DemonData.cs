using UnityEngine;
using System.Collections;

//悪魔達の初期情報(structの代わりにclassを使用)
[System.Serializable]
public class DemonData
{
    //[SerializeField, TooltipAttribute("悪魔の種類")]
    //public GameObject gameObuject;

    [SerializeField, TooltipAttribute("体力")]
    public int HP = 100;
    [SerializeField, TooltipAttribute("攻撃力")]
    public int ATK = 100;
    [SerializeField, TooltipAttribute("速度")]
    public int SPEED = 5;
    [SerializeField, TooltipAttribute("攻撃間隔")]
    public float AtackTime = 1.0f;

    private int MaxHP;
    public int GetMaxHP { get { return MaxHP; } }

    DemonData() { MaxHP = HP; }
}