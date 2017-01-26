using UnityEngine;
using System.Collections;

public class ResultJingle : MonoBehaviour
{
    public AudioSource start;
    public AudioSource loop;

    bool loopstart = false;

    void Start ()
	{
		
	}
	
	void Update ()
	{
		if(!start.isPlaying && !loopstart)
        {
            loopstart = true;
            loop.Play();
        }
	}
}