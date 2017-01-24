using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool pausing;
    
	void Update ()
	{
		if(pausing)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;
	}
}