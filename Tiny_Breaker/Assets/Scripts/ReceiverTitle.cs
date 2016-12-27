using UnityEngine;
using StaticClass;
using UnityEngine.UI;
using SocketIO;

public class ReceiverTitle : MonoBehaviour
{
    SocketIOComponent socket;

    [SerializeField]
    GameObject[] ok = new GameObject[GameRule.playerNum];
    
    public Text text;

    void Start ()
	{
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        
        socket.On("PushRobbyID", SetOkActive);
        socket.On("connect", TextOpen);
    }

    public void SetOkActive(SocketIOEvent e)
	{
        string _PlayerNo = e.data.GetField("PlayerID").str;

        Debug.Log(e.data.GetField("PlayerID").str);

        Debug.Log(System.Convert.ToInt32(_PlayerNo));

        //プレイヤーのID
        ok[System.Convert.ToInt32(_PlayerNo)].SetActive(true);
    }

    void TextOpen(SocketIOEvent e)
    {
        text.text = "Connect";
    }
}