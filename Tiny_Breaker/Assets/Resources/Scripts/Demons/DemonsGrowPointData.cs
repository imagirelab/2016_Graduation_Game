using UnityEngine;

//悪魔達の初期情報(structの代わりにclassを使用)
[System.Serializable]
public class DemonsGrowPointData
{
    public enum Type
    {
        POPO,
        PUPU,
        PIPI
    }
    [SerializeField, TooltipAttribute("種類")]
    private Type DemonType = Type.POPO;
    [SerializeField, TooltipAttribute("体力の成長値")]
    private int HP_GrowPoint = 0;
    [SerializeField, TooltipAttribute("攻撃力の成長値")]
    private int ATK_GrowPoint = 0;
    [SerializeField, TooltipAttribute("速度の成長値")]
    private int SPEED_GrowPoint = 0;
    [SerializeField, TooltipAttribute("攻撃間隔の成長値")]
    private int AtackTime_GrowPoint = 1;

    public Type GetDemonType { get { return DemonType; } }
    public int GetHP_GrowPoint { get { return HP_GrowPoint; } }
    public int GetATK_GrowPoint { get { return ATK_GrowPoint; } }
    public int GetSPEED_GrowPoint { get { return SPEED_GrowPoint; } }
    public int GetAtackTime_GrowPoint { get { return AtackTime_GrowPoint; } }

    //プレハブのすべて共有の値になってしまうため
    //元々のステータスはいじらないようにするため
    //別の変数を用意
    private int currentHP_GrowPoint;
    private int currentATK_GrowPoint;
    private int currentSPEED_GrowPoint;
    private float currentAtackTime_GrowPoint;

    public int CurrentHP_GrowPoint
    {
        get { return currentHP_GrowPoint; }
        set { currentHP_GrowPoint = value; }
    }
    public int CurrentATK_GrowPoint
    {
        get { return currentATK_GrowPoint; }
        set { currentATK_GrowPoint = value; }
    }
    public int CurrentSPEED_GrowPoint
    {
        get { return currentSPEED_GrowPoint; }
        set { currentSPEED_GrowPoint = value; }
    }
    public float CurrentAtackTime_GrowPoint
    {
        get { return currentAtackTime_GrowPoint; }
        set { currentAtackTime_GrowPoint = value; }
    }

    //現在の成長値に代入する
    public void SetGrowPoint()
    {
        currentHP_GrowPoint = HP_GrowPoint;
        currentATK_GrowPoint = ATK_GrowPoint;
        currentSPEED_GrowPoint = SPEED_GrowPoint;
        currentAtackTime_GrowPoint = AtackTime_GrowPoint;
    }

    public int GetCost()
    {
        return currentHP_GrowPoint + currentATK_GrowPoint + currentSPEED_GrowPoint;
    }
}