using System.Collections;
using System.Collections.Generic;
using Difficulty;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Rooms;

namespace TitleScreen {
    public class PlayButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public DifficultyModifierUI difficultyModifierUIPrefab;
        public SpriteRenderer Background;
        public AudioSource song;
        private Color targetColor = Color.white;
        public Color HoverColor;
        public float FadeSpeed = 5f;
        private bool hovering;
        public float pitchChangeRate = 0.1f;
        void Start()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(Press);
            song.time = 20f;
        }

        private void Press() {
            LevelManager.getInstance().reset();
            DifficultyModifierUI difficultyModifierUI = Instantiate(difficultyModifierUIPrefab);
            difficultyModifierUI.transform.SetParent(transform.parent.parent,false);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hovering = false;
            targetColor = Color.white;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hovering = true;
            targetColor = HoverColor;
        }

        public void FixedUpdate() {
            if (hovering) {
                song.pitch += pitchChangeRate;
                song.volume += pitchChangeRate/10;
                if (song.volume > 0.6f) {
                    song.volume = 0.6f;
                }
                if (song.pitch > 3) {
                    song.pitch = 3;
                }
            } else {
                song.volume -= pitchChangeRate/10;
                if (song.volume < 0.3f) {
                    song.volume = 0.3f;
                }
                if (song.pitch < 1) {
                    song.pitch -= pitchChangeRate;
                } else {
                    song.pitch = 1;
                }
            }
            Background.color = Color.Lerp(Background.color, targetColor, FadeSpeed * Time.deltaTime);
        }
    }
}

