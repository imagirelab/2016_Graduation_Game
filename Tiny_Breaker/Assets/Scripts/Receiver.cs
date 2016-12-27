using UnityEngine;
using System.Collections.Generic;
using StaticClass;
using SocketIO;
using UnityEngine.UI;

public class Receiver : MonoBehaviour
{
    SocketIOComponent socket;

    [SerializeField]
    Player[] players = new Player[GameRule.playerNum];
    
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
    
    public Text connect;
    public Text getDemon;
    public Text getDeathblow;

    void Start()
    {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.On("connect", TextOpen);
        socket.On("DemonPushed", GetDemon);
        socket.On("DeadlyPushed", GetDeathblow);
    }

    void Update()
    {
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

        getDemon.text = "GetDemon\n" +
                        "PlayerID : " + _PlayerNo + "\n" +
                        "Type : " + _Type + "\n" +
                        "Direction : " + _Direction + "\n" +
                        "Level : " + _Level;

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
            smaphoMsg.level = System.Convert.ToInt32(_Level);

        smaphoMsgList.Add(smaphoMsg);
    }

    public void GetDeathblow(SocketIOEvent e)
    {
        DeathblowMsg deathblowMsg = new DeathblowMsg();
        string _PlayerNo = e.data.GetField("PlayerID").str;
        string _Deadly = e.data.GetField("Deadly").str;

        getDeathblow.text = "GetDeathblow\n" +
                        "PlayerID : " + _PlayerNo + "\n" +
                        "Deadly : " + _Deadly + "\n";

        //プレイヤーのID
        deathblowMsg.playerID = System.Convert.ToInt32(_PlayerNo);

        //必殺技の種類
        Enum.Deathblow_TYPE deathType = Enum.Deathblow_TYPE.Fire;
        switch (_Deadly)
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
    }

    void TextOpen(SocketIOEvent e)
    {
        connect.text = "Connect";
    }
}