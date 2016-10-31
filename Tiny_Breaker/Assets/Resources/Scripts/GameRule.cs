//ゲーム全体に関わるルール
//一つであることが保証される

using System.Collections.Generic;

namespace StaticClass
{
    public class GameRule
    {
        public bool debugFlag;
        public const int playerNum = 2;
        public const int roundCount = 3;

        public enum ResultType
        {
            Player1Win,
            Player2Win,
            Draw,
            TimeUp,
            None
        }
        //最終結果
        public ResultType result = ResultType.None;
        //ラウンド結果
        public List<ResultType> round = new List<ResultType>();

        static GameRule gameRule = new GameRule();

        GameRule()
        {
            Reset();
        }

        public static GameRule getInstance()
        {
            return gameRule;
        }

        public void Reset()
        {
            //最終結果リセット
            result = ResultType.None;
            //ラウンド結果リセット
            round.Clear();
        }
    }
}
