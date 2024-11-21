using System.Collections;
using System.Collections.Generic;
using Difficulty;
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
        private DifficultyModifier difficulty;
        public DifficultyModifier DifficultyModifier => difficulty;
        private LevelManager() {

        }
        public static LevelManager getInstance() {
            if (instance == null) {
                instance = new LevelManager();
            }
            return instance;
        }

        public void reset() {
            CurrentLevel = null;
            CurrentLevelPrefab = null;
            playerData = null;
        }

        public void setModifier(DifficultyModifier DifficultyModifier)
        {
            this.difficulty = DifficultyModifier;
        }
    }
}

