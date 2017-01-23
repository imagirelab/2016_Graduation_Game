using UnityEngine;
using StaticClass;

public class BGM : MonoBehaviour
{
    public AudioClip[] BackMusics;

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
        //oneBeatTime = Oneminuts / BPM;

        _auido = GetComponent<AudioSource>();

        _auido.time = 0;

        //_auido.Play();

        int i = GameRule.getInstance().round.Count;

        switch (GameRule.getInstance().round.Count)
        {
            case 0:
                _auido.clip = BackMusics[i];

                if (!_auido.isPlaying)
                {
                    _auido.Play();
                }

                if (_auido.time > oneBeatTime * (loopEndtMeasure + 1) * beatType && _auido.isPlaying)
                {
                    _auido.time = oneBeatTime * loopStartMeasure * beatType;
                    _auido.Play();
                }
                break;
            case 1:
                if (!_auido.isPlaying)
                {
                    _auido.clip = BackMusics[i];
                    _auido.Play();
                }
                break;
            case 2:
                if (!_auido.isPlaying)
                {
                    _auido.clip = BackMusics[i];
                    _auido.Play();
                }
                break;
            default:
                break;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        
	}
}
