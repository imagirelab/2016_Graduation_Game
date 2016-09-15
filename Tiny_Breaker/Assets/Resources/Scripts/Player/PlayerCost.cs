using UnityEngine;

public class PlayerCost : MonoBehaviour {

    [SerializeField, TooltipAttribute("最大コスト")]
    int MaxCost = 0;
    public int GetMaxCost { get { return MaxCost; } }

    [SerializeField, TooltipAttribute("初期コスト")]
    int StateCost = 0;

    [SerializeField, TooltipAttribute("毎秒上がるコスト")]
    int CostParSecond = 0;

    [SerializeField, TooltipAttribute("悪魔召喚の初期コスト")]
    int DemonCost = 0;
    public int GetDemonCost { get { return DemonCost; } }

    [SerializeField, TooltipAttribute("兵士の撃破獲得コスト")]
    int SoldierCost = 0;
    public int GetSoldierCost { get { return SoldierCost; } }

    [SerializeField, TooltipAttribute("小屋の撃破獲得コスト")]
    int HouseCost = 0;
    public int GetHouseCost { get { return HouseCost; } }

    //現在のコスト
    int currentCost = 0;
    public int CurrentCost { get { return currentCost; } }

    float time = 0;     //時間
    
    void Start () {
        currentCost = StateCost;
    }
	
	void Update () {
        //毎秒増えるコスト
        if (time >= 1.0f)
        {
            time = 0;

            if (currentCost + CostParSecond <= MaxCost)
                currentCost += CostParSecond;
        }

        //1フレームあたりの時間を取得
        time += Time.deltaTime;
    }

    //コストを足す
    //上限を超えたときは上限値を代入する
    public void AddCost(int addcost)
    {
        if (currentCost + addcost <= MaxCost)
            currentCost += addcost;
        else
            currentCost = MaxCost;
    }

    //コストが使えるかどうか
    //使える場合は数値を引いたのち引けたことを返す
    public bool UseableCost(int cost)
    {

        if (currentCost - cost >= 0)
        {
            currentCost -= cost;
            
            return true;
        }
        else
            return false;
    }

    public void SetDefault(int max, int state, int costparsecond, int demon, int soldier, int house)
    {
        MaxCost = max;
        StateCost = state;
        CostParSecond = costparsecond;
        DemonCost = demon;
        SoldierCost = soldier;
        HouseCost = house;

        Start();
    }
}
