//兵士のデータを溜めるクラス
//シングルトンにしたのはフィールド全体の兵士は、
//プレイヤー全員にとっても共通のものだから

namespace StaticClass
{
    public class SolgierDataBase : DataBase
    {
        static SolgierDataBase dataBase = new SolgierDataBase();
        
        public static SolgierDataBase getInstance()
        {
            return dataBase;
        }
    }
}
