using UnityEngine;
using System.Collections;

public class DemonRefrectSE : MonoBehaviour
{
    public AudioClip refrectSE;

    public float cancelTime = 0.2f;
    float timer = 0;

    AudioSource _audio;

    public static bool refrectSEFlag = false;

    // Use this for initialization
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (refrectSEFlag && timer > cancelTime)
        {
            _audio.clip = refrectSE;
            _audio.Play();
            refrectSEFlag = false;
            timer = 0;
        }
    }
}
