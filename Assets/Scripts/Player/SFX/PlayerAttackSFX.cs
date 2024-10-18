using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerModule {
    public enum AttackSFX {
        Swing,
        Hit
    }
    public class PlayerAttackSFX : MonoBehaviour
    {
        public void PlaySound(Weapon weapon, AttackSFX type) {
            AudioClip clip = weapon.swingSound;
            if (clip == null) {
                return;
            }
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource.isPlaying) {
                return;
            }
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}

