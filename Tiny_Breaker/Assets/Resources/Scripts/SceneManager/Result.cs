using UnityEngine;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
	void Start ()
	{
		
	}
	
	void Update ()
	{
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}