//悪魔のデータを溜めるクラス

namespace StaticClass
{
    public class DemonDataBase : DataBase
    {
        static DemonDataBase dataBase = new DemonDataBase();
        
        public static DemonDataBase getInstance()
        {
            return dataBase;
        }
    }
}
