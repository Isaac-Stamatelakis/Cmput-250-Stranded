using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerModule {
    public class PlayerData
    {
        public int Level;
        public int Experience;
        public float Health;
        public List<PlayerUpgrade> upgrades;
        public Weapon weapon;
        public PlayerStats playerStats;

        public PlayerData(int level, int experience, float health, List<PlayerUpgrade> upgrades, Weapon weapon, PlayerStats playerStats)
        {
            Level = level;
            Experience = experience;
            Health = health;
            this.upgrades = upgrades;
            this.weapon = weapon;
            this.playerStats = playerStats;
        }

        public override string ToString()
        {
            return  $"Level: {Level}|" +
                    $"Experience: {Experience}|" +
                    $"Health: {Health}|" +
                    $"Upgrades: {string.Join(", ", upgrades)}|" +
                    $"Weapon: {weapon.name}";
        }
    }

    public class PlayerStats
    {
        public float Time;
        public float DamageDealt;
        public float DamageTaken;
        public float Healing;
        public int Kills;
    }
    
}

