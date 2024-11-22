using System;
using System.Collections;
using System.Collections.Generic;
using Rooms;
using UnityEngine;

namespace PlayerModule {
    public class Player : MonoBehaviour
    {
        private static Player instance;
        public static Player Instance => instance;
        public DatePlayer DatePlayer;
        private bool inCutscene;
        private bool inDialog;
        public PlayerStats PlayerStats = new PlayerStats();
        public bool CanMove => !inCutscene && !inDialog;
        [SerializeField] private PlayerUI playerUI;
        public PlayerUI PlayerUI => playerUI;
        public void setCutscene(bool inCutscene) {
            this.inCutscene = inCutscene;
        }
        public void setDialog(bool inDialog) {
            this.inDialog = inDialog;
        }
        public void Awake() {
            instance = this;
        }

        public void Update()
        {
            PlayerStats.Time += Time.deltaTime;
        }

        public void SetPosition(Vector3 position) {
            this.transform.position = position;
            DatePlayer.transform.position = position;
        }

        public void Heal(float amount)
        {
            float healModifier = LevelManager.getInstance().DifficultyModifier.GetHealingModifier();
            amount*=healModifier;
            if (GetComponent<PlayerLevelComponent>().HasUpgrade(PlayerUpgrade.Healing))
            {
                amount *= PlayerUpgradeUtils.HEAL_UPGRADE_MODIFIER;
            }
            PlayerStats.Healing += amount;
            GetComponent<PlayerHealth>().Heal(amount);
        }

        public PlayerData serialize() {
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            PlayerLevelComponent playerLevelComponent = GetComponent<PlayerLevelComponent>();
            PlayerAttack playerAttack = GetComponent<PlayerAttack>();
            float health = playerHealth.Health;
            // Prevents healing from increase max health
            if (playerLevelComponent.HasUpgrade(PlayerUpgrade.Health)) {
                health -= PlayerUpgradeUtils.HEALTH_UPGRADE_MODIFER;
            }
            
            return new PlayerData(
                level: playerLevelComponent.PlayerLevel.Level,
                experience: playerLevelComponent.PlayerLevel.Experience,
                health: health,
                upgrades: playerLevelComponent.PlayerUpgrades,
                weapon: playerAttack.currentWeapon,
                playerStats: PlayerStats
            );
        }

        public void unseralize(PlayerData playerData) {
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            PlayerLevelComponent playerLevelComponent = GetComponent<PlayerLevelComponent>();
            PlayerAttack playerAttack = GetComponent<PlayerAttack>();

            playerHealth.setHealth(playerData.Health);
            playerAttack.SetWeapon(playerData.weapon);
            foreach (PlayerUpgrade playerUpgrade in playerData.upgrades) {
                playerLevelComponent.AddPlayerUpgrade(playerUpgrade);
            }
            this.PlayerStats = playerData.playerStats;
            playerLevelComponent.SetExperienceAndLevel(new PlayerLevel(playerData.Experience,playerData.Level));
        }
        

        public void refreshUI() {
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            PlayerLevelComponent playerLevelComponent = GetComponent<PlayerLevelComponent>();

            playerLevelComponent.IterateUpgrades();
            playerHealth.Damage(0);
            playerLevelComponent.AddExperience(0);
        }
    }
}
