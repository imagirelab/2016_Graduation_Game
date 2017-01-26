using UnityEngine;
using System.Collections;

public class SoldierDeadSE : MonoBehaviour
{
    public AudioClip deadSE;

    public float cancelTime = 0.2f;
    float timer = 0;

    AudioSource _audio;

    public static bool soldierDeadSEFlag = false;

    // Use this for initialization
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (soldierDeadSEFlag && timer > cancelTime)
        {
            _audio.clip = deadSE;
            _audio.Play();
            soldierDeadSEFlag = false;
            timer = 0;
        }
    }
}
