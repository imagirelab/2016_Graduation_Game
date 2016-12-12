//ラウンド間で受け渡すデータを集める場所

using UnityEngine;

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


        public void Reset()
        {
            for (int i = 0; i < GameRule.playerNum; i++)
            {
                passesCost[i] = 0;
                passesDeadCount[i] = 0;
                passesKnockDownCount[i] = 0;
                POPOLevel[i] = 0;
                PUPULevel[i] = 0;
                PIPILevel[i] = 0;
            }
        }
    }
}