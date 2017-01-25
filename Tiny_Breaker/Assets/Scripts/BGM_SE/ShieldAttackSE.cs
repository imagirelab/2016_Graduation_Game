using UnityEngine;
using System.Collections;

public class ShieldAttackSE : MonoBehaviour
{
    public AudioClip attackSE;

    public float cancelTime = 0.2f;
    float timer = 0;

    AudioSource _audio;

    public static bool ShieldattackSEFlag = false;

    // Use this for initialization
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (ShieldattackSEFlag && timer > cancelTime)
        {
            _audio.clip = attackSE;
            _audio.Play();
            ShieldattackSEFlag = false;
            timer = 0;
        }
    }
}
