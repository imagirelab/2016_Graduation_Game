﻿using UnityEngine;
using System.Collections;

public class Jingle : MonoBehaviour
{
    public AudioClip musicList;

    AudioSource _auido;

    bool jingleStop = false;

    // Use this for initialization
    void Start ()
    {
        _auido = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(Main.roundEndFlag)
        {
            Debug.Log("call");

            if(_auido.clip != musicList && jingleStop)
            {
                _auido.clip = musicList;
                _auido.Play();
                jingleStop = false;
            }            
        }
        else
        {
            jingleStop = true;
        }
	}
}