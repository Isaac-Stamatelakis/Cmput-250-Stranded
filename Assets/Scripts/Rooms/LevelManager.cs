using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;

namespace Rooms
{
    public class LevelManager
    {
        private static LevelManager instance;
        public PlayerData playerData;
        public Level CurrentLevelPrefab;
        public Level CurrentLevel;
        private LevelManager() {

        }
        public static LevelManager getInstance() {
            if (instance == null) {
                instance = new LevelManager();
            }
            return instance;
        }
    }
}

