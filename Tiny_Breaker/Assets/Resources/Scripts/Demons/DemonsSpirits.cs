using UnityEngine;
using StaticClass;

public class DemonsSpirits : MonoBehaviour
{
    //魂に覚えさせる成長値
    private DemonsGrowPointData growPoint;
    public DemonsGrowPointData GrowPoint
    {
        get { return growPoint; }
        set { growPoint = value; }
    }
    
    // 作られたときにリストに追加して
    void Start()
    {
        SpiritDataBase.getInstance().AddList(this.gameObject);
    }

    //破壊されたときにリストから外す
    void OnDisable()
    {
        SpiritDataBase.getInstance().RemoveList(this.gameObject);
    }
}
