using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource _audio;
    
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        
        if(_audio)
        {
            _audio.Play();
        }
    }
}
