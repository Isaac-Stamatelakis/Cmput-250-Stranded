using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkSFX : MonoBehaviour
{
    public enum PlayerMovementSound {
        Walk,
        Run
    }
    public AudioClip walkSound;
    public AudioClip runSound;
    public void playSound(PlayerMovementSound playerMovementSound) {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource.isPlaying) {
            return;
        }
        switch (playerMovementSound) {
            case PlayerMovementSound.Walk:
                audioSource.clip = walkSound;
                break;
            case PlayerMovementSound.Run:
                audioSource.clip = runSound;
                break;            
        }
        audioSource.Play();
    }
}
