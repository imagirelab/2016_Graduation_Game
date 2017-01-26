using UnityEngine;
using StaticClass;

public class BGM : MonoBehaviour
{
    public AudioClip[] HeadMusics;
    public AudioClip[] BodyMusics;

    AudioSource _auido;

    public float volume = 0.6f;


    void Start ()
    {
        _auido = GetComponent<AudioSource>();

        _auido.time = 0;
        _auido.loop = false;
        _auido.volume = volume;
        
        int i = GameRule.getInstance().round.Count;

        //ラウンド数を見て頭部分の再生
        switch (GameRule.getInstance().round.Count)
        {
            case 0:
                _auido.clip = HeadMusics[i];
                _auido.Play();
                break;
            case 1:
                _auido.clip = HeadMusics[i];
                _auido.Play();
                break;
            case 2:
                _auido.clip = HeadMusics[i];
                _auido.Play();
                break;
            default:
                break;
        }
    }
	
	void Update ()
    {
        if(!_auido.isPlaying)
        {
            _auido.loop = true;

            int i = GameRule.getInstance().round.Count;

            //ラウンド数を見て体部分の再生
            switch (GameRule.getInstance().round.Count)
            {
                case 0:
                    _auido.clip = BodyMusics[i];
                    _auido.Play();
                    break;
                case 1:
                    _auido.clip = BodyMusics[i];
                    _auido.Play();
                    break;
                case 2:
                    _auido.clip = BodyMusics[i];
                    _auido.Play();
                    break;
                default:
                    break;
            }
        }
    }
}
