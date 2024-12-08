using System.Collections;
using System.Collections.Generic;
using Difficulty;
using UnityEngine;
using PlayerModule;

namespace Rooms {
    public class LevelSceneLoader : MonoBehaviour
    {
        public Level defaultLevel;
        public DifficultyPresets testingDifficulty;
        public void Start() {
            LevelManager levelManager = LevelManager.getInstance();
            if (levelManager.CurrentLevelPrefab == null) {
                levelManager.CurrentLevelPrefab = defaultLevel;
            }

            if (levelManager.DifficultyModifier == null)
            {
                levelManager.setModifier(testingDifficulty.GetDifficultyModifier());
            }
            loadCurrentLevel();
            Player.Instance.refreshUI();
   
            GameObject.Destroy(gameObject);
        }

        public static void loadCurrentLevel() {
            LevelManager levelManager = LevelManager.getInstance();
            Level level = GameObject.Instantiate(levelManager.CurrentLevelPrefab);
            levelManager.CurrentLevel = level;
            level.Load();
            Player player = Player.Instance;;
            Debug.Log($"Level {level.name} Loaded");
            if (levelManager.playerData != null) {
                player = Player.Instance;
                player.unseralize(levelManager.playerData);
                Debug.Log($"Unserialized Player Data: {levelManager.playerData}");
            }

            if (levelManager.DifficultyModifier.OneHealthMode)
            {
                player.GetComponent<PlayerHealth>().setMaxHealth(1);
            }
            player.refreshUI();

            WeaponStatsUI weaponStatsUI = WeaponStatsUI.Instance;
            if (weaponStatsUI != null && player.currentWeapon != null)
            {
                weaponStatsUI.UpdateWeaponStats(player.currentWeapon);
            }

        }
    }
}

