using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerModule {

    public enum PlayerHurtSound {
        Damaged,
        Death
    }
    public class PlayerHurtSFX : MonoBehaviour
    {
        [SerializeField] private AudioClip hurt;
        [SerializeField] private AudioClip die;

        public void PlaySound(PlayerHurtSound sound) {
            AudioClip clip = GetClip(sound);
            if (clip == null) {
                return;
            }
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource.isPlaying && sound != PlayerHurtSound.Death) {
                return;
            }
            audioSource.clip = clip;
            audioSource.Play();
        }

        private AudioClip GetClip(PlayerHurtSound sound) {
            switch (sound) {
                case PlayerHurtSound.Damaged:
                    return hurt;
                case PlayerHurtSound.Death:
                    return die;
                default:
                 return null;
            }
        }
    }
}

