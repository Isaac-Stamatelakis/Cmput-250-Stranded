using System;
using System.Collections;
using System.Collections.Generic;
using Rooms;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Difficulty
{
    public class DifficultyModifierUI : MonoBehaviour
    {
        public GridLayoutGroup PresetButtons;
        public Scrollbar ZombieModifier;
        public Scrollbar HealingModifier;
        public Scrollbar BossModifier;
        public Toggle SkipIntroToggle;
        public Toggle OneHealthToggle;
        public Toggle CheckPointToggle;
        public Button CancelButton;
        public Button PlayButton;

        public void Start()
        {

            InitalizeScrollbar(HealingModifier, DifficultyUtils.HealingModifiers);
            InitalizeScrollbar(BossModifier, DifficultyUtils.BossHealthModifier);
            InitalizeScrollbar(ZombieModifier, DifficultyUtils.ZombieModifiers);
            
            var presetButtons = PresetButtons.GetComponentsInChildren<Button>();
            for (int i = 0; i < presetButtons.Length; i++)
            {
                DifficultyPresets preset = (DifficultyPresets)i;
                presetButtons[i].onClick.AddListener(() =>
                {
                    SelectPreset(preset);
                });
            }
            CancelButton.onClick.AddListener(CancelClick);
            PlayButton.onClick.AddListener(PlayClick);
            
            SelectPreset(DifficultyPresets.Normal);
        }

        private void InitalizeScrollbar(Scrollbar scrollbar, List<float> values)
        {
            scrollbar.numberOfSteps = values.Count;
            scrollbar.onValueChanged.AddListener((float value) =>
            {
                TextMeshProUGUI textElement = scrollbar.GetComponentInChildren<TextMeshProUGUI>();
                textElement.text = $"{values[GetScrollBarStepValue(scrollbar)]:F1}x";
            });
        }

        private void PlayClick()
        {
            DifficultyModifier modifier = new DifficultyModifier(
                OneHealthToggle.isOn,
                CheckPointToggle.isOn,
                GetScrollBarStepValue(ZombieModifier),
                GetScrollBarStepValue(HealingModifier),
                    GetScrollBarStepValue(BossModifier)
            );
            LevelManager.getInstance().setModifier(modifier);
            string sceneName = SkipIntroToggle.isOn ? "LevelScene" : "BackgroundStory";
            SceneManager.LoadScene(sceneName);
        }

        private void SetScrollBarStepValue(int step, Scrollbar scrollbar)
        {
            scrollbar.value = ((float)step)/(scrollbar.numberOfSteps-1);
        }

        private int GetScrollBarStepValue(Scrollbar scrollbar)
        {
            return (int) (scrollbar.value * (scrollbar.numberOfSteps-1));
        }

        private void CancelClick()
        {
            Destroy(gameObject);
        }
        private void SelectPreset(DifficultyPresets preset)
        {
            DifficultyModifier modifier = preset.GetDifficultyModifier();
            SetScrollBarStepValue(modifier.HealingModifier, HealingModifier);
            SetScrollBarStepValue(modifier.BossModifier, BossModifier);
            SetScrollBarStepValue(modifier.ZombieModifier, ZombieModifier);

            CheckPointToggle.isOn = modifier.CheckPoints;
            OneHealthToggle.isOn = modifier.OneHealthMode;

        }
    }
}

