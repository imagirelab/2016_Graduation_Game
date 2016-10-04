//ゲーム全体に関わるルール
//一つであることが保証される

namespace StaticClass
{
    public class GameRule
    {
        public bool debugFlag;
        public int playerNum = 2;

        static GameRule gameRule = new GameRule();

        public static GameRule getInstance()
        {
            return gameRule;
        }
    }
}
