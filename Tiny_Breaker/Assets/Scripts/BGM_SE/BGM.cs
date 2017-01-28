using UnityEngine;
using StaticClass;

public class BGM : MonoBehaviour
{
    public AudioClip[] HeadMusics;

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
        _auido.clip = HeadMusics[i];
        _auido.Play();
    }
}
