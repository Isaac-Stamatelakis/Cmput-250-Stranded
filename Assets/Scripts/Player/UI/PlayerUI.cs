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
        [SerializeField] private Scrollbar experienceScrollbar;
        [SerializeField] private TextMeshProUGUI experienceText;
        public void displayHealth(float current, float max) {
            text.text = $"{current} / {max}";
            scrollbar.size = current/max;
        }

        public void displayExperience(int level, float experience, float levelUpExperience) {
            if (experienceText == null || experienceScrollbar == null) {
                Debug.LogWarning("Experience values not set in playerui");
                return;
            }
            experienceText.text = level.ToString();
            experienceScrollbar.size = experience/levelUpExperience;
        }
    }
}

