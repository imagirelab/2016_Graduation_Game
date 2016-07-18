//魂アイコン用のクラス

using UnityEngine;

public class IconSpirit : MonoBehaviour {

    //魂の仮ステータス
    private DemonsGrowPointData growPoint;
    public DemonsGrowPointData GrowPoint
    {
        get { return growPoint; }
        set { growPoint = value; }
    }
}
