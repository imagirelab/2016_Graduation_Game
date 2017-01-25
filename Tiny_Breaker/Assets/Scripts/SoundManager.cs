using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public enum AUDIOTYPE
    {
        atack,
        dead,
        summon,
        refrect,
    }
    public AUDIOTYPE modeType = AUDIOTYPE.atack;

    public AudioClip atackSE;
    public AudioClip deadSE;
    public AudioClip summonSE;
    public AudioClip refrectSE;

    public static bool atackSEFlag = false;
    public static bool deadSEFlag = false;
    public static bool summonSEFlag = false;
    public static bool refrectSEFlag = false;

    private AudioSource _audioSource;   //音楽再生用

    public float cancelTime = 0f;
    private float timer;

    // Use this for initialization
    void Start ()
    {
        _audioSource = this.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;

        if (deadSEFlag && timer > cancelTime && modeType == AUDIOTYPE.dead)
        {
            deadSEFlag = false;
            _audioSource.PlayOneShot(deadSE);
            timer = 0;
        }

        if (atackSEFlag && timer > cancelTime && modeType == AUDIOTYPE.atack)
        {
            atackSEFlag = false;
            _audioSource.PlayOneShot(atackSE);
            timer = 0;
        }

        if (summonSEFlag && timer > cancelTime && modeType == AUDIOTYPE.summon)
        {
            summonSEFlag = false;
            _audioSource.PlayOneShot(summonSE);
            timer = 0;
        }

        if (refrectSEFlag && timer > cancelTime && modeType == AUDIOTYPE.refrect)
        {
            refrectSEFlag = false;
            _audioSource.PlayOneShot(refrectSE);
            timer = 0;
        }
    }
}
