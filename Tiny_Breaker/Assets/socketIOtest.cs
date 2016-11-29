using UnityEngine;
using System.Collections;
using SocketIO;

public class socketIOtest : MonoBehaviour
{
    SocketIOComponent socket;

	// Use this for initialization
	void Start ()
    {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.On("DemonPushed", GetDemon);
	}
	
	public void GetDemon(SocketIOEvent e)
    {
        string _Type = e.data.GetField("Type").str;

        Debug.Log("Type : " + _Type);
    }
}
