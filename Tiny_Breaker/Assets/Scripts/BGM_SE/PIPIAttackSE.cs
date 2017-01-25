using UnityEngine;
using System.Collections;

public class PIPIAttackSE : MonoBehaviour {

    public AudioClip attackSE;

    public float cancelTime = 0.2f;
    float timer = 0;

    AudioSource _audio;

    public static bool PIPIattackSEFlag = false;

    // Use this for initialization
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (PIPIattackSEFlag && timer > cancelTime)
        {
            _audio.clip = attackSE;
            _audio.Play();
            PIPIattackSEFlag = false;
            timer = 0;
        }
    }
}
