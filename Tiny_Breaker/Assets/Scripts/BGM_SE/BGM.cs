using UnityEngine;

public class BGM : MonoBehaviour
{
    public float beatType = 4;

    public float BPM;

    public float loopStartMeasure;
    public float loopEndtMeasure;

    AudioSource _auido;

    private float Oneminuts = 60;

    private float oneBeatTime;

	// Use this for initialization
	void Start ()
    {
        oneBeatTime = Oneminuts / BPM;

        _auido = GetComponent<AudioSource>();

        _auido.Play();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(!_auido.isPlaying)
        {
            _auido.time = oneBeatTime * loopStartMeasure * beatType;
            _auido.Play();
        }

        if(_auido.time > oneBeatTime * (loopEndtMeasure + 1) * beatType && _auido.isPlaying)
        {
            _auido.time = oneBeatTime * loopStartMeasure * beatType;
            _auido.Play();
        }
	}
}
