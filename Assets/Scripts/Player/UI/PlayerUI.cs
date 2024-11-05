using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PlayerModule {
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Scrollbar scrollbar;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private PlayerExperienceUI playerExperienceUI;
        [SerializeField] private PauseScreen pauseScreenPrefab;
        public PlayerExperienceUI PlayerExperienceUI => playerExperienceUI;

        public void displayHealth(float current, float max) {
            text.text = $"{current:F0} / {max:F0}";
            scrollbar.size = current/max;
        }

        public void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Transform canvasTransform = transform.parent;
                PauseScreen pauseScreen = canvasTransform.GetComponentInChildren<PauseScreen>();
                if (pauseScreen != null) {
                    GameObject.Destroy(pauseScreen.gameObject);
                    return;
                }
                PauseScreen newPauseScreen = GameObject.Instantiate(pauseScreenPrefab);
                newPauseScreen.transform.SetParent(canvasTransform,false);
            }
        }
    }
}

