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

        public PlayerData(int level, int experience, float health, List<PlayerUpgrade> upgrades, Weapon weapon)
        {
            Level = level;
            Experience = experience;
            Health = health;
            this.upgrades = upgrades;
            this.weapon = weapon;
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
}

