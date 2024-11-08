using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerModule {
    public enum PlayerLevelSFX {
        LevelUp
    }
    public class PlayerLevelUpSFX : MonoBehaviour
    {

        [SerializeField] private AudioClip levelUpSound;
        public void PlaySoundClip(PlayerLevelSFX levelSFX) {
            AudioClip clip = GetAudioClip(levelSFX);
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

        private AudioClip GetAudioClip(PlayerLevelSFX levelSFX) {
            switch (levelSFX) {
                case PlayerLevelSFX.LevelUp:
                    return levelUpSound;
                default:
                    return null;
            }
        }
    }

}
