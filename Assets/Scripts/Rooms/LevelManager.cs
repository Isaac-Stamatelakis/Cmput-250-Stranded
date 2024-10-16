using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rooms
{
    public class LevelManager
    {
        private static LevelManager instance;
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

