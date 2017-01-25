using UnityEngine;
using System.Collections;

public class PUPUAttackSE : MonoBehaviour {

    public AudioClip attackSE;

    public float cancelTime = 0.2f;
    float timer = 0;

    AudioSource _audio;

    public static bool PUPUattackSEFlag = false;

    // Use this for initialization
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (PUPUattackSEFlag && timer > cancelTime)
        {
            _audio.clip = attackSE;
            _audio.Play();
            PUPUattackSEFlag = false;
            timer = 0;
        }
    }
}
