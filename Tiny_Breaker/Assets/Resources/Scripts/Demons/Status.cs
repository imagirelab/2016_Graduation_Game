using UnityEngine;

//悪魔達の初期情報(structの代わりにclassを使用)
[System.Serializable]
public class Status
{
    [SerializeField, TooltipAttribute("体力")]
    private int HP = 0;
    [SerializeField, TooltipAttribute("攻撃力")]
    private int ATK = 0;
    [SerializeField, TooltipAttribute("速度")]
    private int SPEED = 0;
    [SerializeField, TooltipAttribute("攻撃間隔")]
    private float AtackTime = 0;
    
    public int GetHP { get { return HP; } }
    public int GetATK { get { return ATK; } }
    public int GetSPEED { get { return SPEED; } }
    public float GetAtackTime { get { return AtackTime; } }

    //プレハブのすべて共有の値になってしまうため
    //元々のステータスはいじらないようにするため
    //別の変数を用意
    private int currentHP;
    private int currentATK;
    private int currentSPEED;
    private float currentAtackTime;

    public int CurrentHP
    {
        get { return currentHP; }
        set { currentHP = value; }
    }
    public int CurrentATK
    {
        get { return currentATK; }
        set { currentATK = value; }
    }
    public int CurrentSPEED
    {
        get { return currentSPEED; }
        set { currentSPEED = value; }
    }
    public float CurrentAtackTime
    {
        get { return currentAtackTime; }
        set { currentAtackTime = value; }
    }

    Status()
    {
        SetStutas();
    }

    //現在のステータスに代入する
    public void SetStutas()
    {
        currentHP = HP;
        currentATK = ATK;
        currentSPEED = SPEED;
        currentAtackTime = AtackTime;
    }
}