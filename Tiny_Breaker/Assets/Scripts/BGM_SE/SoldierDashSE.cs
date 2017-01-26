using UnityEngine;
using System.Collections;

public class SoldierDashSE : MonoBehaviour
{
    public AudioClip DashSE;

    public float cancelTime = 0.2f;
    float timer = 0;

    AudioSource _audio;

    public static bool SoldierDashSEFlag = false;

    // Use this for initialization
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (SoldierDashSEFlag && timer > cancelTime)
        {
            _audio.clip = DashSE;
            _audio.Play();
            SoldierDashSEFlag = false;
            timer = 0;
        }
    }
}
