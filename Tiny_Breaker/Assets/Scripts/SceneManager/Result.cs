using UnityEngine;
using UnityEngine.SceneManagement;
using SocketIO;

public class Result : MonoBehaviour
{
	void Start ()
    {
        SocketIOComponent socket;

        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.On("GameEndRequest", GameEnd);

        socket.Emit("StopEndRequest");
    }

    void Update ()
	{
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LodaScene();
        }
    }

    void GameEnd(SocketIOEvent e)
    {
        LodaScene();
    }

    void LodaScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}