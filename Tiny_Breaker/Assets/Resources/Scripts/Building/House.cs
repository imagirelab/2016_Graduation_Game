using UnityEngine;
using StaticClass;

public class House : MonoBehaviour {

    //家のステータス
    [SerializeField, TooltipAttribute("体力")]
    int HP = 1000;
    int currentHP = 0;

    //このクラス内で使う変数
    private GameObject HP_UI;           //HPのUI

    //外から見れる変数
    public int HPpro { get { return currentHP; } set { currentHP = value; } }

    bool IsDead = false;
    
    void Start ()
    {
        // 作られたときにリストに追加する
        BuildingDataBase.getInstance().AddList(this.gameObject);

        currentHP = HP;

        HP_UI = transform.FindChild("HP").gameObject;
    }

    //破壊されたときにリストから外す
    void OnDisable()
    {
        if(!IsDead)
            BuildingDataBase.getInstance().RemoveList(this.gameObject);
    }

    void Update ()
    {
        HP_UI.GetComponent<TextMesh>().text = "HP: " + currentHP.ToString();

        if(currentHP <= 0)
        {
            //HPリセット
            currentHP = HP;
        }
    }
}
