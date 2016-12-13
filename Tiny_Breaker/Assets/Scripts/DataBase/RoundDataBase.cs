//ラウンド間で受け渡すデータを集める場所
namespace StaticClass
{
    public class RoundDataBase
    {
        static RoundDataBase dataBase = new RoundDataBase();

        public static RoundDataBase getInstance()
        {
            return dataBase;
        }

        //受け渡すコスト
        int[] passesCost = new int[GameRule.playerNum];
        public int[] PassesCost { get { return passesCost; } set { passesCost = value; } }

        //倒された数
        int[] passesDeadCount = new int[GameRule.playerNum];
        public int[] PassesDeadCount { get { return passesDeadCount; } set { passesDeadCount = value; } }

        //倒した数
        int[] passesKnockDownCount = new int[GameRule.playerNum];
        public int[] PassesKnockDownCount { get { return passesKnockDownCount; } set { passesKnockDownCount = value; } }

        //悪魔達のレベル
        public int[] POPOLevel = new int[GameRule.playerNum];
        public int[] PUPULevel = new int[GameRule.playerNum];
        public int[] PIPILevel = new int[GameRule.playerNum];

        int startLevel = 0;

        public void Reset()
        {
            for (int i = 0; i < GameRule.playerNum; i++)
            {
                passesCost[i] = 0;

                passesDeadCount[i] = 0;
                passesKnockDownCount[i] = 0;
                POPOLevel[i] = startLevel;
                PUPULevel[i] = startLevel;
                PIPILevel[i] = startLevel;
            }
        }

        public void AddPassesDeadCount(string tag)
        {
            switch(tag)
            {
                case "Player1":
                    passesDeadCount[0]++;
                    break;
                case "Player2":
                    passesDeadCount[1]++;
                    break;
                default:
                    break;
            }
        }

        public void AddPassesKnockDownCount(string tag)
        {
            switch (tag)
            {
                case "Player1":
                    passesKnockDownCount[0]++;
                    break;
                case "Player2":
                    passesKnockDownCount[1]++;
                    break;
                default:
                    break;
            }
        }
        
        public void SetDemonLevel(string tag, int popo, int pupu, int pipi)
        {
            switch (tag)
            {
                case "Player1":
                    POPOLevel[0] = popo;
                    PUPULevel[0] = pupu;
                    PIPILevel[0] = pipi;
                    break;
                case "Player2":
                    POPOLevel[1] = popo;
                    PUPULevel[1] = pupu;
                    PIPILevel[1] = pipi;
                    break;
                default:
                    break;
            }
        }
    }
}