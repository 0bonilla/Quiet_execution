using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManger : MonoBehaviour
{
    public AudioSource musicSource; 

    void Start()
    {     
        musicSource.Play();
    }
}
