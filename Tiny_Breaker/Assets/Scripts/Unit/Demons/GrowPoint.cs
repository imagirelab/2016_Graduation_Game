using UnityEngine;

//悪魔達の初期情報(structの代わりにclassを使用)
[System.Serializable]
public class GrowPoint
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
    private int currentAtackTime_GrowPoint;
    private int level = 0;

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
    public int CurrentAtackTime_GrowPoint
    {
        get { return currentAtackTime_GrowPoint; }
        set { currentAtackTime_GrowPoint = value; }
    }
    public int Level
    {
        get { return level; }
    }

    public GrowPoint()
    {
        SetGrowPoint();
    }

    //現在の成長値に代入する
    public void SetGrowPoint()
    {
        currentHP_GrowPoint = HP_GrowPoint;
        currentATK_GrowPoint = ATK_GrowPoint;
        currentSPEED_GrowPoint = SPEED_GrowPoint;
        currentAtackTime_GrowPoint = AtackTime_GrowPoint;

        //レベル初期化
        level = 0;
    }

    //成長値の合計値の取得
    public int GetCost()
    {
        return currentHP_GrowPoint + currentATK_GrowPoint + currentSPEED_GrowPoint;
    }

    //成長値を足す
    public void AddGrowPoint(GrowPoint addPoint)
    {
        //成長値の足し方
        currentHP_GrowPoint += HP_GrowPoint + addPoint.GetHP_GrowPoint;
        currentATK_GrowPoint += ATK_GrowPoint + addPoint.GetATK_GrowPoint;
        currentSPEED_GrowPoint += SPEED_GrowPoint + addPoint.GetSPEED_GrowPoint;
        currentAtackTime_GrowPoint += AtackTime_GrowPoint + addPoint.GetAtackTime_GrowPoint;

        //レベルアップ
        level++;
    }
    
    //基準を変えたいときに呼び出す
    public void SetDefault(Type type, int hp, int atk, int speed, int atkspeed)
    {
        DemonType = type;

        HP_GrowPoint = hp;
        ATK_GrowPoint = atk;
        SPEED_GrowPoint = speed;
        AtackTime_GrowPoint = atkspeed;

        SetGrowPoint();
    }
}