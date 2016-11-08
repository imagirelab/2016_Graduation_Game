namespace Loader
{
    public class CostData : DataLode<CostMaster>
    {
    }

    public class CostMaster : MasterBase
    {
        public int Round { get; private set; }
        public int MaxCost { get; private set; }
        public int StateCost { get; private set; }
        public int CostParSecond { get; private set; }
        public int DemonCost { get; private set; }
        public float DemonCostRate { get; private set; }
        public int SoldierCost { get; private set; }
        public int HouseCost { get; private set; }
    }
}
