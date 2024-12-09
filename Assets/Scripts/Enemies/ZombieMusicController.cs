using System.Collections;
using UnityEngine;

public class ZombieMusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hurtSound;
    public float minInterval = 2f;  // Minimum time between random sounds
    public float maxInterval = 5f; // Maximum time between random sounds
    public float minPitch = 0.7f; // Minimum volume
    public float maxPitch = 0.9f;   // Maximum volume
    public void OnZombieHurt()
    {
        PlayHurtSound(Random.Range(minPitch, maxPitch));
    }

    private void PlayHurtSound(float pitch)
    {
        if (hurtSound != null && audioSource != null)
        {
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(hurtSound);
        }
    }
}
