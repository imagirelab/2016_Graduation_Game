namespace Loader
{
    public class GrowData : DataLode<GrowMaster>
    {
    }

    public class GrowMaster : MasterBase
    {
        public string ID { get; private set; }
        public string NAME { get; private set; }
        public int GHP { get; private set; }
        public int GATK { get; private set; }
        public int GSPEED { get; private set; }
        public int GATKSPEED { get; private set; }
    }
}
