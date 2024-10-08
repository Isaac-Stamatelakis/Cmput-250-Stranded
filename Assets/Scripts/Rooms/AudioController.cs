using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private static AudioController _instance;
    public static AudioController Instance { get { return _instance; } }

    public AudioSource audioSource; 
    public AudioSource backgroundAudioSource; 
    public AudioClip playerWalking;  
    public AudioClip playerRunning; 
    public AudioClip backgroundMusic; 

    void Awake()
    {
        
        _instance = this;  
    }

    
    void Start()
    {
        
        PlayBackgroundMusic();
    }

    
    public void PlayPlayerWalking()
    {
        if (!audioSource.isPlaying || audioSource.clip != playerWalking)
        {
            audioSource.clip = playerWalking;
            audioSource.loop = true; 
            audioSource.Play();
        }
    }

    public void PlayPlayerRunning()
    {
        if (!audioSource.isPlaying || audioSource.clip != playerRunning)
        {
            audioSource.clip = playerRunning;
            audioSource.loop = true; 
            audioSource.Play();
        }
    }

    public void StopPlayerWalking()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundAudioSource != null && backgroundMusic != null)
        {
            backgroundAudioSource.clip = backgroundMusic;
            backgroundAudioSource.loop = true; 
            backgroundAudioSource.Play();
        }
    }

    // Method to stop the background music if needed
    public void StopBackgroundMusic()
    {
        if (backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.Stop();
        }
    }
}
