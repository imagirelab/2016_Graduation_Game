//小屋
//HPは取り合う量

using UnityEngine;
using StaticClass;

public class House : MonoBehaviour
{

    //家のステータス
    [SerializeField, TooltipAttribute("体力")]
    int HP = 1000;
    int currentHP = 0;
    [SerializeField, TooltipAttribute("自動回復量")]
    int regene = 0;

    //このクラス内で使う変数
    //private GameObject HP_UI;           //HPのUI

    //外から見れる変数
    public int HPpro { get { return currentHP; } set { currentHP = value; } }
    public int GetHP { get { return HP; } }

    bool IsDead = false;

    [HideInInspector]
    public bool IsDamage = false;
    int oldHP = 0;

    void Start()
    {
        // 作られたときにリストに追加する
        BuildingDataBase.getInstance().AddList(this.gameObject);

        currentHP = 0;

        //HP_UI = transform.FindChild("DebugHP").gameObject;
    }

    //破壊されたときにリストから外す
    void OnDisable()
    {
        if (!IsDead)
            BuildingDataBase.getInstance().RemoveList(this.gameObject);
    }

    void Update()
    {
        //HP_UI.GetComponent<TextMesh>().text = "HP: " + currentHP.ToString();

        DmageCheck(currentHP);

        switch (transform.gameObject.tag)
        {
            case "Player1":
                //自動回復
                currentHP += regene;
                //HPリセット
                if (currentHP >= HP)
                    currentHP = HP;
                break;
            case "Player2":
                //自動回復
                currentHP -= regene;
                //HPリセット
                if (currentHP <= -HP)
                    currentHP = -HP;
                break;
            default:
                break;
        }
    }

    void DmageCheck(int nowHP)
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
