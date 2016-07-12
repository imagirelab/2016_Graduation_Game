using UnityEngine;

public class DemonsSpirits : MonoBehaviour
{
    //プレイヤーの仮ステータス
    private DemonsGrowPointData growPoint;
    public DemonsGrowPointData GrowPoint
    {
        get { return growPoint; }
        set { growPoint = value; }
    }
}
