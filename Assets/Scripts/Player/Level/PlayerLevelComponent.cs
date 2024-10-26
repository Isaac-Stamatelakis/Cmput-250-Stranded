using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace PlayerModule {
    public enum PlayerUpgrade {
        Attack,
        Health,
        Speed,
        Healing,
        DateAura,
        DateHeal,
        DateAttack
    }

    public static class PlayerUpgradeExtension {
        public static string getDescription(this PlayerUpgrade playerUpgrade) {
            switch (playerUpgrade) {
                case PlayerUpgrade.Attack:
                    return "+Attack\nDeal an additional 20% damage!";
                default:
                    return "";
            }
        }
    }
    public class PlayerLevelComponent : MonoBehaviour
    {
        private PlayerUI playerUI;
        private PlayerLevel playerLevel = new PlayerLevel();
        private HashSet<PlayerUpgrade> playerUpgrades = new HashSet<PlayerUpgrade>();
        private List<PlayerUpgrade> nextSelectableUpgrades;
        public List<PlayerUpgrade> SelectableUpgrades => nextSelectableUpgrades;
        public bool hasUpgrade(PlayerUpgrade playerUpgrade) {
            return playerUpgrades.Contains(playerUpgrade);
        }
        public void Start() {
            playerUI = Player.Instance.PlayerUI;
            nextSelectableUpgrades = generateSelectableUpgrades();
            playerUI.PlayerExperienceUI.displayLevelUpOption();
        }

        private List<PlayerUpgrade> generateSelectableUpgrades() {
            int NUMBER_OF_CHOICES = 2;
            PlayerUpgrade[] enumValues = (PlayerUpgrade[])Enum.GetValues(typeof(PlayerUpgrade));
            List<PlayerUpgrade> unselected = new List<PlayerUpgrade>();
            foreach (PlayerUpgrade playerUpgrade in enumValues) {
                if (playerUpgrades.Contains(playerUpgrade)) {
                    continue;
                }
                unselected.Add(playerUpgrade);
            }
            if (unselected.Count < NUMBER_OF_CHOICES) {
                return unselected;
            }
            System.Random random = new System.Random();
            return unselected
                .OrderBy(n => random.Next())
                .Take(NUMBER_OF_CHOICES)
                .ToList();
        }

        public void iterateUpgrades() {
            nextSelectableUpgrades = generateSelectableUpgrades();
        }


        public void addExperience(int amount) {
            bool leveledUp = playerLevel.addExperience(amount);
            playerUI.PlayerExperienceUI.displayExperience(playerLevel.Level,playerLevel.Experience,playerLevel.getLevelUpExperience());
            if (leveledUp) {
                playerUI.PlayerExperienceUI.displayLevelUpOption();
            }
        }

        public void addComponent(PlayerUpgrade playerUpgrade) {
            playerUpgrades.Add(playerUpgrade);
        }
        
    }

    public class PlayerLevel {
        public int Experience;
        public int Level;
        // Adds experience. Returns true if player levels up
        public bool addExperience(int amount) {
            Experience += amount;
            int levelUpRequirement = getLevelUpExperience();
            if (Experience >= levelUpRequirement) {
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

