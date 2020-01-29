﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTesting : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioClip[] music;
    public float volume,pitch;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.instance.PLaySFX(clips[Random.Range(0, clips.Length)], volume, pitch);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioManager.instance.PlaySong(music[Random.Range(0, music.Length)]);
        }
    }
}
