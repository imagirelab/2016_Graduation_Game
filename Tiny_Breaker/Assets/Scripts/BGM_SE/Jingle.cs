using UnityEngine;
using System.Collections;

public class Jingle : MonoBehaviour
{
    public AudioClip musicList;

    public float jingleVolume = 0.7f;

    AudioSource _auido;

    bool jingleStop = false;

    // Use this for initialization
    void Start ()
    {
        _auido = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(Main.roundEndFlag)
        {
            if(_auido.clip != musicList && jingleStop)
            {
                _auido.clip = musicList;
                _auido.volume = jingleVolume;
                _auido.Play();
                jingleStop = false;
            }            
        }
        else
        {
            jingleStop = true;
        }
	}
}
