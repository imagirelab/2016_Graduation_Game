using UnityEngine;

public class DeadSE : MonoBehaviour
{
    private AudioSource playingChecker;


	// Use this for initialization
	void Start ()
    {
        playingChecker = GetComponent<AudioSource>();
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!playingChecker.isPlaying)
        {
            Destroy(this.gameObject);
        }
	}
}
