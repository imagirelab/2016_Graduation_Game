using UnityEngine;
using System.Collections;

public class DemonDeadSE : MonoBehaviour
{
    public AudioClip deadSE;

    public float cancelTime = 0.2f;
    float timer = 0;

    AudioSource _audio;

    public static bool deadSEFlag = false;

    // Use this for initialization
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (deadSEFlag && timer > cancelTime)
        {
            _audio.clip = deadSE;
            _audio.Play();
            deadSEFlag = false;
            timer = 0;
        }
    }
}
