using UnityEngine;
using System.Collections;

public class Title_BGM : MonoBehaviour
{
    public AudioClip headBGM;
    public AudioClip bodyBGM;

    AudioSource _auido;

    bool headPlayed;

    public float volume = 0.6f;

    // Use this for initialization
    void Start ()
    {
        _auido = GetComponent<AudioSource>();

        _auido.loop = false;
        _auido.volume = volume;

        _auido.clip = headBGM;
        _auido.time = 0f;
        _auido.Play();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(_auido.time >= headBGM.length && !headPlayed)
        {
            headPlayed = true;
        }

        if (headPlayed && !_auido.loop)
        {
            _auido.loop = true;
            _auido.clip = bodyBGM;
            _auido.time = 0f;
            _auido.Play();
        }            
    }
}
