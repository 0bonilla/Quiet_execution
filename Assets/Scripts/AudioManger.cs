using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManger : MonoBehaviour
{
    public AudioSource musicSource; 

    // Inicia la musica
    void Start()
    {     
        musicSource.Play();
    }
}
