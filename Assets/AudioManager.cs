using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Keeps this object across scenes
    }

    public void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        Debug.Log($"Playing 2D sound: {clip.name}");

        // Create a temporary GameObject to play the sound
        GameObject tempAudioObject = new GameObject("TempAudio");

        AudioSource audioSource = tempAudioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.spatialBlend = 0.0f; // Makes it 2D
        audioSource.Play();

        // Destroy the temp audio object after the clip finishes
        Destroy(tempAudioObject, clip.length);
    }
}
