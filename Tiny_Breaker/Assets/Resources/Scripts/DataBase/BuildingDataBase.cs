//建物のデータを溜めるクラス

namespace StaticClass
{
    public class BuildingDataBase : DataBase
    {
        static BuildingDataBase dataBase = new BuildingDataBase();

        public static BuildingDataBase getInstance()
        {
            return dataBase;
        }
    }
}
