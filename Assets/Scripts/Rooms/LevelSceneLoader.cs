using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rooms {
    public class LevelSceneLoader : MonoBehaviour
    {
        public Level defaultLevel;
        public void Start() {
            LevelManager levelManager = LevelManager.getInstance();
            if (levelManager.CurrentLevelPrefab == null) {
                levelManager.CurrentLevelPrefab = defaultLevel;
            }
            Level level = GameObject.Instantiate(levelManager.CurrentLevelPrefab);
            levelManager.CurrentLevel = level;
            level.Load();
            Debug.Log($"Level {level.name} Loaded");
            GameObject.Destroy(gameObject);
        }
    }
}

