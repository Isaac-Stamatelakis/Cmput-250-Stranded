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
        DamageReduction,
        DateAura,
        DateHeal,
        DateAttack
    }

    public static class PlayerUpgradeUtils {
        public static readonly float DAMAGE_UPGRADE_MODIFIER = 1.2f;
        public static readonly float SPEED_UPGRADE_MODIFIER = 1.25f;
        public static int HEALTH_UPGRADE_MODIFER = 25;
        public static float HEAL_UPGRADE_MODIFIER = 1.5f;
        public static float DAMAGE_REDUCTION_MODIFIER = 0.8f;
        public static int DATE_KILL_HEAL_COUNT = 5;
        public static int DATE_KILL_HEAL_AMOUNT = 10;
        public static string formatPercentIncrease(float val, bool inverse=false) {
            float percent;
            if (inverse) {
                percent = 1-val;
            } else {
                percent = val-1;
            }
            return (percent).ToString("P0").Replace(" ","");
        }
    }

    public static class PlayerUpgradeExtension {
        public static string getDescription(this PlayerUpgrade playerUpgrade) {
            switch (playerUpgrade) {
                case PlayerUpgrade.Attack:
                    return $"Attack Increase\nDeal an additional {PlayerUpgradeUtils.formatPercentIncrease(PlayerUpgradeUtils.DAMAGE_UPGRADE_MODIFIER)} damage!";
                case PlayerUpgrade.Health:
                    return $"Health Increase\nGain {PlayerUpgradeUtils.HEALTH_UPGRADE_MODIFER} health";
                case PlayerUpgrade.Speed:
                    return $"Speed Increase\nMove {PlayerUpgradeUtils.formatPercentIncrease(PlayerUpgradeUtils.SPEED_UPGRADE_MODIFIER)} faster!";
                case PlayerUpgrade.Healing:
                    return $"Healing Increase\nHeal {PlayerUpgradeUtils.formatPercentIncrease(PlayerUpgradeUtils.HEAL_UPGRADE_MODIFIER)} more!";
                case PlayerUpgrade.DamageReduction:
                    return $"Damage Reduction\nReduce Damage by {PlayerUpgradeUtils.formatPercentIncrease(PlayerUpgradeUtils.DAMAGE_REDUCTION_MODIFIER,true)}!";
                case PlayerUpgrade.DateAura:
                    return $"Date Aura\nStand near your date for a buff!";
                case PlayerUpgrade.DateHeal:
                    return $"Date Heal\nKill {PlayerUpgradeUtils.DATE_KILL_HEAL_COUNT} enemies for your date to heal you {PlayerUpgradeUtils.DATE_KILL_HEAL_AMOUNT} health!";
                case PlayerUpgrade.DateAttack:
                    return $"Date Attack\nYour date aids you in combat!";
                default:
                    return "";
            }
        }
    }
    public class PlayerLevelComponent : MonoBehaviour
    {
        [SerializeField] private Material dateAuraShader;
        private Material defaultShader;
        private PlayerLevel playerLevel = new PlayerLevel();
        public PlayerLevel PlayerLevel => playerLevel;
        private List<PlayerUpgrade> playerUpgrades = new List<PlayerUpgrade>();
        public List<PlayerUpgrade> PlayerUpgrades => playerUpgrades;
        private List<PlayerUpgrade> nextSelectableUpgrades;
        public List<PlayerUpgrade> SelectableUpgrades => nextSelectableUpgrades;
        public int RemainingUpgrades => playerLevel.Level-playerUpgrades.Count;
        private bool dateAura = false;
        public bool DateAura => dateAura;
        public bool hasUpgrade(PlayerUpgrade playerUpgrade) {
            return playerUpgrades.Contains(playerUpgrade);
        }
        public void Start() {
            nextSelectableUpgrades = generateSelectableUpgrades();
            defaultShader = GetComponent<SpriteRenderer>().sharedMaterial;
            addExperience(250000);
        }

        public void setExperienceAndLevel(PlayerLevel playerLevel) {
            this.playerLevel = playerLevel;
        }

        public void setDateAura(bool state) {
            if (state == dateAura) {
                return;
            }
            this.dateAura = state;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.material = dateAura ? dateAuraShader : defaultShader;
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
            PlayerUI playerUI = Player.Instance.PlayerUI;
            playerUI.PlayerExperienceUI.displayExperience(playerLevel.Level,playerLevel.Experience,playerLevel.getLevelUpExperience());
            if (leveledUp) {
                playerUI.PlayerExperienceUI.displayLevelUpOption();
            }
        }

        public void addComponent(PlayerUpgrade playerUpgrade) {
            playerUpgrades.Add(playerUpgrade);
            switch (playerUpgrade) {
                case PlayerUpgrade.Health:
                    Player.Instance.GetComponent<PlayerHealth>().IncreaseHealth(PlayerUpgradeUtils.HEALTH_UPGRADE_MODIFER);
                    break;
            }
            // Only has an effect with certain upgrades
            Player.Instance.DatePlayer.activeUpgrade(playerUpgrade);

            PlayerUI playerUI = Player.Instance.PlayerUI;
            playerUI.PlayerExperienceUI.displayUpgrade(playerUpgrade);
        }
        
    }

    public class PlayerLevel {
        public int Experience;
        public int Level;

        public PlayerLevel()
        {
        }

        public PlayerLevel(int experience, int level)
        {
            Experience = experience;
            Level = level;
        }

        // Adds experience. Returns true if player levels up
        public bool addExperience(int amount) {
            Experience += amount;
            int levelRequirement = getLevelUpExperience();
            bool leveledUp = false;
            while (Experience >= levelRequirement) {
                Experience -= levelRequirement;
                Level++;
                levelRequirement = getLevelUpExperience();
                leveledUp = true;
            }
            return leveledUp;
        }

        public int getLevelUpExperience() {
            return 2*Level*Level + 10*Level + 25;
        }
    }
}

