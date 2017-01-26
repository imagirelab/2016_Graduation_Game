using UnityEngine;
using System.Collections;

public class RollEnd : MonoBehaviour
{
    public AudioClip rollEndSE;

    AudioSource _audio;

    public static bool rollEndSEFlag = false;

    // Use this for initialization
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rollEndSEFlag)
        {
            _audio.clip = rollEndSE;
            _audio.Play();
            rollEndSEFlag = false;
        }
    }
}
