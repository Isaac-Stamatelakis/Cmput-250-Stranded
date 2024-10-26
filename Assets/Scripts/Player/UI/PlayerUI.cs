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
        public PlayerExperienceUI PlayerExperienceUI => playerExperienceUI;

        public void displayHealth(float current, float max) {
            text.text = $"{current} / {max}";
            scrollbar.size = current/max;
        }
    }
}

