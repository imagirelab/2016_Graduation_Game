using UnityEngine;

public class DeadParticleToObject : MonoBehaviour
{
    public ParticleSystem playEndChecker;
    
    void Update()
    {
        if (!playEndChecker.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}