using UnityEngine;
using UnityEngine.UI;

public class DefenseBase : MonoBehaviour
{
    //HP
    [SerializeField, TooltipAttribute("体力")]
    private int HP = 1000;
    int currentHP = 0;

    public int GetHP { get { return HP; } }
    public int HPpro { get { return currentHP; } set { currentHP = value; } }
    
    [HideInInspector]
    public bool IsDamage = false;

    int oldHP = 0;

    //ポットの持ち主
    public Player potPlayer;
    public int damageCost = 25;
    
    void Start()
    {
        currentHP = HP;
    }

    void Update()
    {
        DamageCheck(currentHP);

        if (currentHP <= 0)
        {
            currentHP = 0;
        }

        if(IsDamage)
        {
            //コストの計算
            if (potPlayer != null)
            {
                potPlayer.AddCostList(damageCost);
            }
        }
    }

    public void DamageCheck(int nowHP)
    {
        if (nowHP < oldHP)
        {
            IsDamage = true;
        }
        else
        {
            IsDamage = false;
        }
        oldHP = nowHP;
    }
}
