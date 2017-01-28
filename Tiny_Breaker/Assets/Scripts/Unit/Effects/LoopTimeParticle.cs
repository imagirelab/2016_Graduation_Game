using UnityEngine;

public class LoopTimeParticle : MonoBehaviour
{
    ParticleSystem particle;

    public float time = 1.0f;
    float nowTime = 0.0f;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();

        nowTime = time;
    }

    void Update()
    {
        nowTime += Time.deltaTime;

        if (nowTime >= time)
        {
            nowTime = 0.0f;

            if (particle.isPlaying)
                particle.Stop();
            particle.time = 0;
            particle.Play();
        }
    }
}