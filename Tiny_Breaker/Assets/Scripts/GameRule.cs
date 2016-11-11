//ゲーム全体に関わるルール
//一つであることが保証される

using UnityEngine;
using System.Collections.Generic;

//ゲームに関わる列挙型
public class Enum : MonoBehaviour
{
    //悪魔の種類
    public enum Demon_TYPE
    {
        POPO,
        PUPU,
        PIPI,
        Num,
        None
    }

    //ルートの方向
    public enum Direction_TYPE
    {
        Bottom,
        Middle,
        Top
    }
    
    public enum Deathblow_TYPE
    {
        Fire
    }

    //ゲーム結果
    public enum ResultType
    {
        Player1Win,
        Player2Win,
        Draw,
        TimeUp,
        None
    }
}

namespace StaticClass
{
    public class GameRule
    {
        public bool debugFlag;
        public const int playerNum = 2;
        public const int roundCount = 3;
        
        //最終結果
        public Enum.ResultType result = Enum.ResultType.None;
        //ラウンド結果
        public List<Enum.ResultType> round = new List<Enum.ResultType>();

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
            result = Enum.ResultType.None;
            //ラウンド結果リセット
            round.Clear();
        }
    }
}
