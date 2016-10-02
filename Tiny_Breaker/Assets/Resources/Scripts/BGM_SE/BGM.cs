using UnityEngine;

public class BGM : MonoBehaviour
{
    public float loopStarttime;
    public float loopEndttime;

    AudioSource _auido;

	// Use this for initialization
	void Start ()
    {
        _auido = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(!_auido.isPlaying)
        {
            _auido.time = loopStarttime;
            _auido.Play();
        }

        if(_auido.time > loopEndttime && _auido.isPlaying)
        {
            _auido.time = loopStarttime;
            _auido.Play();
        }
	}
}
