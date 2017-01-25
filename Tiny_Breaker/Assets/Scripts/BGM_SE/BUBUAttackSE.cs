using UnityEngine;
using System.Collections;

public class BUBUAttackSE : MonoBehaviour {

    public AudioClip attackSE;

    public float cancelTime = 0.2f;
    float timer = 0;

    AudioSource _audio;

    public static bool BUBUattackSEFlag = false;

    // Use this for initialization
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (BUBUattackSEFlag && timer > cancelTime)
        {
            _audio.clip = attackSE;
            _audio.Play();
            BUBUattackSEFlag = false;
            timer = 0;
        }
    }
}
