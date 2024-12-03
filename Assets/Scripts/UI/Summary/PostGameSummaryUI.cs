using System;
using System.Collections;
using System.Collections.Generic;
using Difficulty;
using PlayerModule;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

namespace SummaryPage
{
    public class PostGameSummaryUI : MonoBehaviour
    {
        public TextMeshProUGUI TimeTaken;
        public TextMeshProUGUI DamageDealt;
        public TextMeshProUGUI DamageTaken;
        public TextMeshProUGUI Deaths;
        public TextMeshProUGUI Healing;
        public TextMeshProUGUI EnemiesKilled;
        public TextMeshProUGUI Congrats;
        public Button ContinueButton;

        public void DisplaySummary(PlayerData playerData, DifficultyModifier modifier, int deaths)
        {
            PlayerStats playerStats = playerData.playerStats;
            AssignText(TimeTaken, $"{playerStats.Time:F1}");
            AssignText(DamageDealt, $"{playerStats.DamageDealt:F1}");
            AssignText(DamageTaken, $"{playerStats.DamageTaken:F1}");
            AssignText(Healing, $"{playerStats.Healing:F1}");
            AssignText(EnemiesKilled, playerStats.Kills.ToString());
            AssignText(Deaths, deaths.ToString());
            
            DifficultyPresets? preset = null;
            DifficultyPresets[] presets = Enum.GetValues(typeof(DifficultyPresets)).Cast<DifficultyPresets>().ToArray();
            
            foreach (DifficultyPresets possiblePreset in presets)
            {
                if (!possiblePreset.GetDifficultyModifier().Equals(modifier)) continue;
                preset = possiblePreset;
                break;
            }
            
            string difficultyText = preset == null ? "A Custom" : preset.ToString();
            Congrats.text = Congrats.text.Replace("{DIFFICULTY}", difficultyText);

            ContinueButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("TitleScene");
            });
        }

        private void AssignText(TextMeshProUGUI textMeshProUGUI, string value)
        {
            textMeshProUGUI.text = textMeshProUGUI.text.Replace("{VAL}", value);
        }
    }
}

