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
    
    private float oneBeatTime = 0.0f;

    int oldCount = 0;


    void Start ()
    {
        _auido = GetComponent<AudioSource>();

        _auido.time = 0;
        
        int i = GameRule.getInstance().round.Count;

        switch (GameRule.getInstance().round.Count)
        {
            case 0:
                _auido.clip = BackMusics[i];

                _auido.Play();

                if (_auido.time > oneBeatTime * (loopEndtMeasure + 1) * beatType && _auido.isPlaying)
                {
                    _auido.time = oneBeatTime * loopStartMeasure * beatType;
                    _auido.Play();
                }
                break;
            case 1:
                _auido.clip = BackMusics[i];
                _auido.Play();
                break;
            case 2:
                _auido.clip = BackMusics[i];
                _auido.Play();
                break;
            default:
                break;
        }
    }
	
	void Update ()
    {

    }
}
