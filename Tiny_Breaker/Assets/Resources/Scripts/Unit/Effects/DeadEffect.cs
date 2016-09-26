using UnityEngine;
using System.Collections;

public class DeadEffect : MonoBehaviour
{
    ParticleSystem playEndChecker;

	// Use this for initialization
	void Start ()
    {
        playEndChecker = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(!playEndChecker.isPlaying)
        {
            Destroy(this.gameObject);
        }
	}
}
