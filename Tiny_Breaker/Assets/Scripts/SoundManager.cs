using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public enum AUDIOTYPE
    {
        find,
        atack,
        dead,
    }
    public AUDIOTYPE modeType = AUDIOTYPE.atack;

    public AudioClip findSE;
    public AudioClip atackSE;
    public AudioClip deadSE;

    public static bool findSEFlag = false;
    public static bool atackSEFlag = false;
    public static bool deadSEFlag = false;

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

        if (findSEFlag && timer > cancelTime && modeType == AUDIOTYPE.find)
        {
            findSEFlag = false;
            _audioSource.PlayOneShot(findSE);
            timer = 0;
        }
	}
}
