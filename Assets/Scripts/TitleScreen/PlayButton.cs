using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace TitleScreen {
    public class PlayButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public SpriteRenderer Background;
        private Color targetColor = Color.white;
        public Color HoverColor;
        public float FadeSpeed = 5f;
        private static readonly string SCENE_NAME = "IsaacLevelScene";
        void Start()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(loadScene);
        }

        private void loadScene() {
            SceneManager.LoadScene(SCENE_NAME);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            targetColor = Color.white;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            targetColor = HoverColor;
        }

        public void FixedUpdate() {
            Background.color = Color.Lerp(Background.color, targetColor, FadeSpeed * Time.deltaTime);
        }
    }
}

