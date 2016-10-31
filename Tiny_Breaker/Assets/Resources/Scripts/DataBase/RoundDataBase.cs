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
        
        public void Reset()
        {
            for(int i = 0; i < passesCost.Length; i++)
                passesCost[i] = 0;
        }
    }
}