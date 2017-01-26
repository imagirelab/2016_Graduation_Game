using UnityEngine;
using System.Collections;

public class SummonSE : MonoBehaviour
{
    public AudioClip _summonSE;

    public float cancelTime = 0.2f;
    float timer = 0;

    AudioSource _audio;

    public static bool SummonSEFlag = false;

    // Use this for initialization
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (SummonSEFlag && timer > cancelTime)
        {
            _audio.clip = _summonSE;
            _audio.Play();
            SummonSEFlag = false;
            timer = 0;
        }
    }
}
