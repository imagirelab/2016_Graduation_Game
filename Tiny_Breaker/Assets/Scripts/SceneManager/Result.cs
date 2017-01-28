using UnityEngine;
using UnityEngine.SceneManagement;
using SocketIO;

public class Result : MonoBehaviour
{
    int count = 0;

	void Start ()
    {
        count = 0;

        SocketIOComponent socket;

        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.On("FinalEnd", GameEnd);

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
        count++;

        if(count >= 2)
            LodaScene();
    }

    void LodaScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}