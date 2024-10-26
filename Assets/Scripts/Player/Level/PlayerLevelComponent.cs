using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerModule {
    public enum PlayerUpgrades {
        Attack,
        Health,
        Speed,
        DamageReduction,
        
    }
    public class PlayerLevelComponent : MonoBehaviour
    {
        private PlayerUI playerUI;
        private PlayerLevel playerLevel = new PlayerLevel();
        private HashSet<PlayerUpgrades> playerUpgrades;
        public void Start() {
            playerUI = GetComponent<PlayerUI>();
        }

        public void addExperience(int amount) {
            playerLevel.addExperience(amount);
            playerUI.displayExperience(playerLevel.Level,playerLevel.Experience,playerLevel.getLevelUpExperience());
        }
    }

    public class PlayerLevel {
        public int Experience;
        public int Level;
        // Adds experience. Returns true if player levels up
        public bool addExperience(int amount) {
            Experience += amount;
            int levelUpRequirement = getLevelUpExperience();
            if (Experience > levelUpRequirement) {
                Experience -= levelUpRequirement;
                Level++;
                return true;
            }
            return false;
        }

        public int getLevelUpExperience() {
            return 2*Level*Level + 10*Level + 25;
        }
    }
}

