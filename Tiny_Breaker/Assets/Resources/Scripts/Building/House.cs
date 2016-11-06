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
    [SerializeField, TooltipAttribute("自動回復時間")]
    float regeneTime = 0.0f;
    float regeneCount = 0.0f;
    
    //外から見れる変数
    public int HPpro { get { return currentHP; } set { currentHP = value; } }
    public int GetHP { get { return HP; } }

    //崩れる小屋のために一個前のタグを保存するもの
    string oldTag = "";
    public string OldTag { get { return oldTag; } set { oldTag = value; } }

    [HideInInspector]
    public bool IsDead = false;     //死んだフラグ
    bool IsDying = false;           //死んでいるフラグ
    [SerializeField]
    float DyingTime = 1.0f;
    float DyingCount = 0.0f;
    GameObject breakObj;

    [HideInInspector]
    public bool IsDamage = false;
    int oldHP = 0;

    void Start()
    {
        // 作られたときにリストに追加する
        BuildingDataBase.getInstance().AddList(this.gameObject);
        
        currentHP = 0;
        regeneCount = 0;

        IsDead = false;
        IsDying = false;

        DyingCount = 0.0f;

        //初めのタグ登録
        oldTag = transform.gameObject.tag;
    }

    //破壊されたときにリストから外す
    void OnDisable()
    {
        BuildingDataBase.getInstance().RemoveList(this.gameObject);
    }

    void Update()
    {
        if (IsDead)
        {
            //死んだとされていて死んでいるフラグが立っていないときの処理
            if(!IsDying)
                Dead();
            //死んでいる間の処理
            else
            {
                DyingCount += Time.deltaTime;

                if(DyingCount >= DyingTime)
                {
                    IsDead = false;
                    IsDying = false;
                    DyingCount = 0.0f;

                    //壊れない小屋を表示
                    foreach (Transform child in transform)
                        if (child.name == "Model")
                            foreach (Transform groundChild in child)
                                if (groundChild.name == "House")
                                    groundChild.gameObject.SetActive(true);

                    //壊れる小屋を消す
                    Destroy(breakObj);

                    // 作られたときにリストに追加する
                    BuildingDataBase.getInstance().AddList(this.gameObject);
                }
            }
        }
        else
        {
            //DmageCheck(currentHP);

            regeneCount += Time.deltaTime;

            if (regeneCount >= regeneTime)
            {
                regeneCount = 0;

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

    //死んだときの処理
    void Dead()
    {
        //死んでいるフラグを立てる
        IsDying = true;

        BuildingDataBase.getInstance().RemoveList(this.gameObject);

        //一旦出ていた兵士は全員殺す
        foreach (Transform child in transform)
            if (child.gameObject.GetComponent<Unit>())
                child.gameObject.GetComponent<Unit>().status.CurrentHP = 0;

        //壊れない小屋を非表示
        foreach (Transform child in transform)
            if (child.name == "Model")
                foreach (Transform groundChild in child)
                    if (groundChild.name == "House")
                        groundChild.gameObject.SetActive(false);

        //壊れる小屋を生成
        breakObj = (GameObject)Instantiate(Resources.Load("Prefabs/House/Break/" + transform.name + "_break"),
                                                            Vector3.zero,
                                                            Quaternion.identity);

        //親はHouseがあるGameObject
        breakObj.transform.SetParent(this.transform, false);
    }

    public void SetDefault(int hp, int regene, float regeneTime)
    {
        this.HP = hp;
        this.regene = regene;
        this.regeneTime = regeneTime;

        currentHP = 0;
        regeneCount = 0;
    }
}
