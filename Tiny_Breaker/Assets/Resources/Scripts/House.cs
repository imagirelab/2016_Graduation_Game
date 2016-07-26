using UnityEngine;
using StaticClass;

public class House : MonoBehaviour {

    //家のステータス
    [SerializeField, TooltipAttribute("体力")]
    private int HP = 1000;

    //このクラス内で使う変数
    private GameObject HP_UI;           //HPのUI

    //外から見れる変数
    public int HPpro { get { return HP; } set { HP = value; } }

    bool IsDead = false;
    
    // Use this for initialization
    void Start ()
    {
        // 作られたときにリストに追加する
        BuildingDataBase.getInstance().AddList(this.gameObject);

        HP_UI = transform.FindChild("HP").gameObject;
    }

    //破壊されたときにリストから外す
    void OnDisable()
    {
        if(!IsDead)
            BuildingDataBase.getInstance().RemoveList(this.gameObject);
    }

    void Update () {

        HP_UI.GetComponent<TextMesh>().text = "HP: " + HP.ToString();

        if (HP <= 0)
        {
            IsDead = true;
             
            BuildingDataBase.getInstance().RemoveList(this.gameObject);

            //いらない子供から消していく
            if (transform.IsChildOf(transform))
                foreach (Transform child in transform)
                    Destroy(child.gameObject);
            Destroy(gameObject);
        }
    }
}
