using UnityEngine;
using System.Collections.Generic;
using StaticClass;
using NCMB;
using SocketIO;

public class Receiver : MonoBehaviour
{
    SocketIOComponent socket;
    public static bool getState = true;

    [SerializeField]
    Player[] players = new Player[GameRule.playerNum];

    [SerializeField]
    bool IsReceive = true;  //受信が必要ないときにOFFにする

    //スマホからのメッセージ一覧
    struct SmaphoMsg
    {
        // Player1 は 0
        // Player2 は 1
        public int playerID;
        public Enum.Direction_TYPE dirType;
        public Enum.Demon_TYPE type;
        public int level;
    };
    List<SmaphoMsg> smaphoMsgList = new List<SmaphoMsg>();
    //スマホからの必殺技メッセージ一覧
    struct DeathblowMsg
    {
        public int playerID;
        public Enum.Deathblow_TYPE type;
    }
    List<DeathblowMsg> DeathblowList = new List<DeathblowMsg>();

    //スマホ側から受け取る情報を抑制するためのカウンター
    [SerializeField]
    int counterLimit = 30;
    int counter = 0;

    void Start()
    {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.On("DemonPushed", GetDemon);
    }

    void Update()
    {
        //受信制御
        counter++;

        if (counter > counterLimit)
        {
            counter = 0;

            //受信後の処理
            if (IsReceive)   //trueの時だけ受信する
                Receive();
        }

        //リストの初めのやつから処理を行う
        if (smaphoMsgList.Count > 0)
        {
            //レベルの設定
            players[smaphoMsgList[0].playerID].SetDemonLevel((int)smaphoMsgList[0].type, (int)smaphoMsgList[0].level);
            //方向指示
            players[smaphoMsgList[0].playerID].ChangeRoot((int)smaphoMsgList[0].dirType);
            //召喚
            players[smaphoMsgList[0].playerID].DebugSummon((int)smaphoMsgList[0].type);
            //処理したらリストから外す
            smaphoMsgList.Remove(smaphoMsgList[0]);
        }

        if (DeathblowList.Count > 0)
        {
            //必殺技発動
            players[DeathblowList[0].playerID].Deathblow();

            //処理したらリストから外す
            DeathblowList.Remove(DeathblowList[0]);
        }
    }

    public void GetDemon(SocketIOEvent e)
    {
        SmaphoMsg smaphoMsg = new SmaphoMsg();
        string _PlayerNo = e.data.GetField("PlayerID").str;
        string _Type = e.data.GetField("Type").str;
        string _Direction = e.data.GetField("Direction").str;
        string _Level = e.data.GetField("Level").str;

        smaphoMsg.playerID = System.Convert.ToInt32(_PlayerNo);

        switch(_Direction)
        {
            case "Top":
                smaphoMsg.dirType = Enum.Direction_TYPE.Top;
                break;
            case "Middle":
                smaphoMsg.dirType = Enum.Direction_TYPE.Middle;
                break;
            case "Bottom":
                smaphoMsg.dirType = Enum.Direction_TYPE.Bottom;
                break;
            default:
                Debug.Log("Player.cs Receive() ncmbObj[Direction] Exception");
                break;
        }

        switch(_Type)
        {
            case "POPO":
                smaphoMsg.type = Enum.Demon_TYPE.POPO;
                break;
            case "PIPI":
                smaphoMsg.type = Enum.Demon_TYPE.PIPI;
                break;
            case "PUPU":
                smaphoMsg.type = Enum.Demon_TYPE.PUPU;
                break;
            default:
                Debug.Log("Player.cs Receive() ncmbObj[Type] Exception");
                break;
        }

        if(_Level != null)
        {
            smaphoMsg.level = System.Convert.ToInt32(_Level);
        }

        smaphoMsgList.Add(smaphoMsg);
        
        getState = false;
    }

    //受信したときの処理
    void Receive()
    {
        #region DemonData

        ////クエリを作成
        //NCMBQuery<NCMBObject> demonStatus = new NCMBQuery<NCMBObject>("DemonData");

        ////検索
        //demonStatus.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        //{
        //    //検索失敗時
        //    if (e != null)
        //    {
        //        Debug.Log(e.ToString());
        //        return;
        //    }
        //    else
        //    {
        //        foreach (NCMBObject ncmbObj in objList)
        //        {
        //            SmaphoMsg smaphoMsg = new SmaphoMsg();

        //            //プレイヤーのID
        //            if (ncmbObj["PlayerNo"] != null)
        //                smaphoMsg.playerID = System.Convert.ToInt32(ncmbObj["PlayerNo"].ToString());

        //            //方向
        //            Enum.Direction_TYPE dir = Enum.Direction_TYPE.Middle;
        //            switch (ncmbObj["Direction"].ToString())
        //            {
        //                case "Top":
        //                    dir = Enum.Direction_TYPE.Top;
        //                    break;
        //                case "Middle":
        //                    dir = Enum.Direction_TYPE.Middle;
        //                    break;
        //                case "Bottom":
        //                    dir = Enum.Direction_TYPE.Bottom;
        //                    break;
        //                default:
        //                    Debug.Log("Player.cs Receive() ncmbObj[Direction] Exception");
        //                    break;
        //            }
        //            smaphoMsg.dirType = dir;

        //            //タイプの振り分け
        //            Enum.Demon_TYPE type = Enum.Demon_TYPE.POPO;
        //            switch (ncmbObj["Type"].ToString())
        //            {
        //                case "POPO":
        //                    type = Enum.Demon_TYPE.POPO;
        //                    break;
        //                case "PUPU":
        //                    type = Enum.Demon_TYPE.PUPU;
        //                    break;
        //                case "PIPI":
        //                    type = Enum.Demon_TYPE.PIPI;
        //                    break;
        //                default:
        //                    Debug.Log("Player.cs Receive() ncmbObj[Type] Exception");
        //                    break;
        //            }
        //            smaphoMsg.type = type;

        //            //レベルの代入
        //            if (ncmbObj["Level"] != null)
        //                smaphoMsg.level = System.Convert.ToInt32(ncmbObj["Level"].ToString());

        //            smaphoMsgList.Add(smaphoMsg);

        //            //記録を取ったら消す
        //            ncmbObj.DeleteAsync();

        //            getState = false;
        //        }
        //    }
        //});

        #endregion

        #region Deathblow

        //クエリを作成
        NCMBQuery<NCMBObject> deathblow = new NCMBQuery<NCMBObject>("DeadlyData");

        //検索
        deathblow.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            //検索失敗時
            if (e != null)
            {
                Debug.Log(e.ToString());
                return;
            }
            else
            {
                foreach (NCMBObject ncmbObj in objList)
                {
                    DeathblowMsg deathblowMsg = new DeathblowMsg();

                    //プレイヤーのID
                    if (ncmbObj["PlayerNo"] != null)
                        deathblowMsg.playerID = System.Convert.ToInt32(ncmbObj["PlayerNo"].ToString());

                    //必殺技の種類
                    Enum.Deathblow_TYPE deathType = Enum.Deathblow_TYPE.Fire;
                    switch (ncmbObj["Type"].ToString())
                    {
                        case "Fire":
                            deathType = Enum.Deathblow_TYPE.Fire;
                            break;
                        default:
                            Debug.Log("Player.cs Receive() ncmbObj[Direction] Exception");
                            break;
                    }
                    deathblowMsg.type = deathType;
                    
                    DeathblowList.Add(deathblowMsg);

                    //記録を取ったら消す
                    ncmbObj.DeleteAsync();
                }
            }
        });

        #endregion
    }
}