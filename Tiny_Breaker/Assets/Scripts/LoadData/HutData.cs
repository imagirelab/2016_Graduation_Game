namespace Loader
{
    public class HutData : DataLode<HutMaster>
    {
    }

    public class HutMaster : MasterBase
    {
        public int No { get; private set; }
        public int SpawnNum { get; private set; }
        public int SpawnMax { get; private set; }
        public float SpawnTime { get; private set; }
        public int HP { get; private set; }
        public int Regene { get; private set; }
        public float RegeneTime { get; private set; }
        public int Level { get; private set; }
    }
}
