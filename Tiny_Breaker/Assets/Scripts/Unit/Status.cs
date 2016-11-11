using UnityEngine;

//悪魔達の初期情報(structの代わりにclassを使用)
[System.Serializable]
public class Status
{
    //基本ステータス
    [SerializeField, TooltipAttribute("体力")]
    private int HP = 0;
    [SerializeField, TooltipAttribute("攻撃力")]
    private int ATK = 0;
    [SerializeField, TooltipAttribute("速度")]
    private float SPEED = 0;
    [SerializeField, TooltipAttribute("攻撃間隔")]
    private float AtackTime = 0;
    
    public int GetHP { get { return HP; } }
    public int GetATK { get { return ATK; } }
    public float GetSPEED { get { return SPEED; } }
    public float GetAtackTime { get { return AtackTime; } }

    //プレハブのすべて共有の値になってしまうため
    //元々のステータスはいじらないようにするため
    //別の変数を用意
    private int maxHP = 0;                  // HP           の最大値
    private int currentHP = 0;              // HP           の現在値
    private int currentATK = 0;             // ATK          の現在値
    private float currentSPEED = 0;         // SPEED        の現在値
    private float currentAtackTime = 0;     // AtackTime    の現在値

    public int MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }
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
    public float CurrentSPEED
    {
        get { return currentSPEED; }
        set { currentSPEED = value; }
    }
    public float CurrentAtackTime
    {
        get { return currentAtackTime; }
        set { currentAtackTime = value; }
    }

    public Status()
    {
        SetStatus(0);
    }

    ////レベルに対応したステータスに設定する
    public void SetStatus(int level)
    {
        currentHP = HP;
        currentATK = ATK;
        currentSPEED = SPEED;
        currentAtackTime = AtackTime;

        //今のステータスを算出する
        float hp = currentHP;
        float atk = currentATK;

        for (int i = 0; i < level; i++)
        {
            hp *= 1.1f;
            atk *= 1.1f;
        }

        currentHP = (int)hp;
        currentATK = (int)atk;
        
        MaxHP = currentHP;
    }

    //基準を変えたいときに呼び出す
    public void SetDefault(int hp, int atk, float speed, float atkspeed)
    {
        HP = hp;
        ATK = atk;
        SPEED = speed;
        AtackTime = atkspeed;
    }
}