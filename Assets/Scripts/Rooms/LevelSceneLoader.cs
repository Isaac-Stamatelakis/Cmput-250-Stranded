using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;

namespace Rooms {
    public class LevelSceneLoader : MonoBehaviour
    {
        public Level defaultLevel;
        public void Start() {
            LevelManager levelManager = LevelManager.getInstance();
            if (levelManager.CurrentLevelPrefab == null) {
                levelManager.CurrentLevelPrefab = defaultLevel;
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
            Debug.Log($"Level {level.name} Loaded");
            if (levelManager.playerData != null) {
                Player player = Player.Instance;
                player.unseralize(levelManager.playerData);
                Debug.Log($"Unserialized Player Data: {levelManager.playerData}");
            }
            Player.Instance.refreshUI();
        }
    }
}

