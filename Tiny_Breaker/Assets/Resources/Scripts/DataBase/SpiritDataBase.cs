//魂のデータを溜めるクラス
//シングルトンにしたのはフィールド全体に落ちている魂は、
//プレイヤー全員にとっても共通のものだから

namespace StaticClass
{
    public class SpiritDataBase : DataBase
    {
        static SpiritDataBase dataBase = new SpiritDataBase();

        public static SpiritDataBase getInstance()
        {
            return dataBase;
        }
    }
}
