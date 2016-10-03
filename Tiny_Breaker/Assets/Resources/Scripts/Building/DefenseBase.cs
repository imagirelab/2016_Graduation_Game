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
    
    //表示するテキスト
    [SerializeField]
    Text Text = null;

    [HideInInspector]
    public bool IsDamage = false;

    int oldHP = 0;

    void Start()
    {
        currentHP = HP;
    }

    void Update()
    {
        DmageCheck(currentHP);

        if (currentHP <= 0)
        {
            //文字表示
            Text.enabled = true;
            currentHP = 0;
        }
    }

    public void DmageCheck(int nowHP)
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
