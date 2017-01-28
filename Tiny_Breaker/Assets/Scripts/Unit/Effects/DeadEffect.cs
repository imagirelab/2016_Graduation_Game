using UnityEngine;

public class DeadEffect : MonoBehaviour
{
    ParticleSystem playEndChecker;
    
	void Start ()
    {
        playEndChecker = GetComponent<ParticleSystem>();
	}
	
	void Update ()
    {
	    if(!playEndChecker.isPlaying)
        {
            Destroy(this.gameObject);
        }
	}
}
