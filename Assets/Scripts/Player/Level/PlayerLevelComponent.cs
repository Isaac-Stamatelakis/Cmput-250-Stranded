using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace PlayerModule {
    /// <summary>
    /// Represents a player upgrade
    /// </summary>
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
    /// <summary>
    /// Various utils and hard coded values for player upgrade system
    /// </summary>
    public static class PlayerUpgradeUtils {
        public static readonly float DAMAGE_UPGRADE_MODIFIER = 1.2f;
        public static readonly float SPEED_UPGRADE_MODIFIER = 1.25f;
        public static readonly int HEALTH_UPGRADE_MODIFER = 25;
        public static readonly float HEAL_UPGRADE_MODIFIER = 1.5f;
        public static readonly float DAMAGE_REDUCTION_MODIFIER = 0.8f;
        public static readonly int DATE_KILL_HEAL_COUNT = 5;
        public static readonly int DATE_KILL_HEAL_AMOUNT = 10;
        public static readonly int DATE_AURA_RANGE = 4;
        public static readonly int DATE_ATTACK_DAMAGE = 8;
        public static readonly int DATE_ATTACK_RATE = 2;
        public static readonly int NUMBER_OF_CHOICES = 3;
        public static string FormatPercentIncrease(float val, bool inverse=false) {
            float percent;
            if (inverse) {
                percent = 1-val;
            } else {
                percent = val-1;
            }
            return (percent).ToString("P0").Replace(" ","");
        }
    }
    /// <summary>
    /// Provides descriptions for a given player upgrade enum
    /// </summary>
    public static class PlayerUpgradeExtension {
        public static string GetDescription(this PlayerUpgrade playerUpgrade) {
            switch (playerUpgrade) {
                case PlayerUpgrade.Attack:
                    return $"Strength Increase\nDeal an additional {PlayerUpgradeUtils.FormatPercentIncrease(PlayerUpgradeUtils.DAMAGE_UPGRADE_MODIFIER)} Damage!";
                case PlayerUpgrade.Health:
                    return $"Health Increase\nGain {PlayerUpgradeUtils.HEALTH_UPGRADE_MODIFER} Health";
                case PlayerUpgrade.Speed:
                    return $"Coffee\nMove {PlayerUpgradeUtils.FormatPercentIncrease(PlayerUpgradeUtils.SPEED_UPGRADE_MODIFIER)} Faster!";
                case PlayerUpgrade.Healing:
                    return $"Healing Increase\nHeal {PlayerUpgradeUtils.FormatPercentIncrease(PlayerUpgradeUtils.HEAL_UPGRADE_MODIFIER)} More!";
                case PlayerUpgrade.DamageReduction:
                    return $"Pet Rock\nReduce Damage Taken by {PlayerUpgradeUtils.FormatPercentIncrease(PlayerUpgradeUtils.DAMAGE_REDUCTION_MODIFIER,true)}!";
                case PlayerUpgrade.DateAura:
                    return $"Date Aura\nStand Near Your Date for a Buff!\n" +
                           $"Deal an Additional {PlayerUpgradeUtils.FormatPercentIncrease(PlayerUpgradeUtils.DAMAGE_UPGRADE_MODIFIER)} Damage and " +
                           $"Reduce Damage Taken by {PlayerUpgradeUtils.FormatPercentIncrease(PlayerUpgradeUtils.DAMAGE_REDUCTION_MODIFIER,true)}!";
                case PlayerUpgrade.DateHeal:
                    return $"Increased Love\nKill {PlayerUpgradeUtils.DATE_KILL_HEAL_COUNT} Enemies For Your Date to Heal you {PlayerUpgradeUtils.DATE_KILL_HEAL_AMOUNT} Health!";
                case PlayerUpgrade.DateAttack:
                    return $"Date Attack\n" +
                           $"Every {PlayerUpgradeUtils.DATE_ATTACK_RATE} Seconds Your Date Throws Her Book " +
                           $"Dealing {PlayerUpgradeUtils.DATE_ATTACK_DAMAGE} Damage!";
                default:
                    return "";
            }
        }
    }
    /// <summary>
    /// Handles logic for player upgrade system
    /// </summary>
    public class PlayerLevelComponent : MonoBehaviour
    {
        [SerializeField] private Material dateAuraShader;
        [SerializeField] private PlayerLevelUpSFX sfx;
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
        /// <summary>
        /// Returns true if the player has a given upgrade
        /// </summary>
        /// <param name="playerUpgrade">Upgrade to check</param>
        /// <returns></returns>
        public bool HasUpgrade(PlayerUpgrade playerUpgrade) {
            return playerUpgrades.Contains(playerUpgrade);
        }
        public void Start() {
            defaultShader = GetComponent<SpriteRenderer>().sharedMaterial;
        }
        
        /// <summary>
        /// Sets the data structure PlayerLevel which stores both player experience and playe level to a given value
        /// </summary>
        /// <param name="playerLevel">Value to set</param>
        public void SetExperienceAndLevel(PlayerLevel playerLevel) {
            this.playerLevel = playerLevel;
        }
        
        /// <summary>
        /// Toggles the player aura buff on and off
        /// </summary>
        /// <param name="state">on/off for aura buff</param>
        public void SetDateAura(bool state) {
            if (state == dateAura) {
                return;
            }
            this.dateAura = state;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.material = dateAura ? dateAuraShader : defaultShader;
        }
        
        /// <summary>
        /// Randomly generates a hard coded number of upgrades that the player can select from
        /// when they level up
        /// </summary>
        /// <returns>Upgrades the player can select</returns>
        public List<PlayerUpgrade> GenerateSelectableUpgrades() {
            PlayerUpgrade[] enumValues = (PlayerUpgrade[])Enum.GetValues(typeof(PlayerUpgrade));
            List<PlayerUpgrade> unselected = new List<PlayerUpgrade>();
            foreach (PlayerUpgrade playerUpgrade in enumValues) {
                if (playerUpgrades.Contains(playerUpgrade)) {
                    continue;
                }
                unselected.Add(playerUpgrade);
            }
            if (unselected.Count < PlayerUpgradeUtils.NUMBER_OF_CHOICES) {
                return unselected;
            }
            System.Random random = new System.Random();
            return unselected
                .OrderBy(n => random.Next())
                .Take(PlayerUpgradeUtils.NUMBER_OF_CHOICES)
                .ToList();
        }
        
        /// <summary>
        /// Updates selectable upgrades from pool of unchosen upgrades
        /// </summary>
        public void IterateUpgrades() {
            nextSelectableUpgrades = GenerateSelectableUpgrades();
        }

        /// <summary>
        /// Adds experience to the player
        /// Levels the player up if amount required amount to level up
        /// </summary>
        /// <param name="amount">Amount to add</param>
        public void AddExperience(int amount) {
            bool leveledUp = playerLevel.AddExperience(amount);
            PlayerUI playerUI = Player.Instance.PlayerUI;
            playerUI.PlayerExperienceUI.displayExperience(playerLevel.Level,playerLevel.Experience,playerLevel.GetLevelUpExperience());
            if (leveledUp) {
                sfx.PlaySoundClip(PlayerLevelSFX.LevelUp);
                playerUI.PlayerExperienceUI.displayLevelUpOption();
            }
        }
        
        /// <summary>
        /// Gives the player a given upgrade
        /// </summary>
        /// <param name="playerUpgrade">Player Upgrade to Give</param>
        public void AddPlayerUpgrade(PlayerUpgrade playerUpgrade) {
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
    /// <summary>
    /// Stores data about a players level and experience
    /// </summary>
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
        public bool AddExperience(int amount) {
            Experience += amount;
            int levelRequirement = GetLevelUpExperience();
            bool leveledUp = false;
            while (Experience >= levelRequirement) {
                Experience -= levelRequirement;
                Level++;
                levelRequirement = GetLevelUpExperience();
                leveledUp = true;
            }
            return leveledUp;
        }
        public int GetLevelUpExperience() {
            return 2*Level*Level + 10*Level + 25;
        }
    }
}

