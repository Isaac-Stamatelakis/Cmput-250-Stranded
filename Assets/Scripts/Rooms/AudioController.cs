using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private static AudioController _instance;
    public static AudioController Instance { get { return _instance; } }

    public AudioSource audioSource; // The AudioSource component to play player sounds
    public AudioSource backgroundAudioSource; // A separate AudioSource for the background sound
    public AudioClip playerWalking;  // The walking sound clip
    public AudioClip playerRunning;  // The running sound clip
    public AudioClip backgroundMusic;  // The background music sound clip

    void Awake()
    {
        // Singleton pattern to ensure only one instance of AudioController exists
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);  // Keeps the AudioController active across scenes if needed
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Play background music at the start
        PlayBackgroundMusic();
    }

    // Method to play the player walking sound
    public void PlayPlayerWalking()
    {
        if (!audioSource.isPlaying || audioSource.clip != playerWalking)
        {
            audioSource.clip = playerWalking;
            audioSource.loop = true; // Loop the sound while walking
            audioSource.Play();
        }
    }

    // Method to play the player running sound
    public void PlayPlayerRunning()
    {
        if (!audioSource.isPlaying || audioSource.clip != playerRunning)
        {
            audioSource.clip = playerRunning;
            audioSource.loop = true; // Loop the sound while running
            audioSource.Play();
        }
    }

    // Method to stop playing the walking or running sound
    public void StopPlayerWalking()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // Method to play the background music
    public void PlayBackgroundMusic()
    {
        if (backgroundAudioSource != null && backgroundMusic != null)
        {
            backgroundAudioSource.clip = backgroundMusic;
            backgroundAudioSource.loop = true; // Loop the background music
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
