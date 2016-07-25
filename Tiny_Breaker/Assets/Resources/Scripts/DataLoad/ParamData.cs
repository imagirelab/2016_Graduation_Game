namespace Loader
{
    public class ParamData : DataLode<ParamMaster>
    {
    }

    public class ParamMaster : MasterBase
    {
        public string Type { get; private set; }
        public string ID { get; private set; }
        public string NAME { get; private set; }
        public int HP { get; private set; }
        public int ATK { get; private set; }
        public int SPEED { get; private set; }
        public int ATKSPEED { get; private set; }
        public float ATKRENGE { get; private set; }
    }
}