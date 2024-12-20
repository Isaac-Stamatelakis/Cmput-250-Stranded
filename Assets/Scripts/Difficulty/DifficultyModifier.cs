using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Difficulty
{
    public class DifficultyModifier
    {
        public readonly bool OneHealthMode;
        public readonly bool CheckPoints;
        public readonly int ZombieModifier;
        public readonly int HealingModifier;
        public readonly int BossModifier;

        public DifficultyModifier(bool oneHealthMode, bool checkPoints, int zombieModifier, int healingModifier, int bossModifier)
        {
            OneHealthMode = oneHealthMode;
            CheckPoints = checkPoints;
            ZombieModifier = zombieModifier;
            HealingModifier = healingModifier;
            BossModifier = bossModifier;
        }

        public float GetZombieModifier()
        {
            return DifficultyUtils.ZombieModifiers[ZombieModifier];
        }

        public float GetBossHealthModifier()
        {
            return DifficultyUtils.BossHealthModifier[BossModifier];
        }

        public float GetBossSpeedModifier()
        {
            return DifficultyUtils.BossSpeedModifier[BossModifier];
        }

        public float GetHealingModifier()
        {
            return DifficultyUtils.HealingModifiers[HealingModifier];
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is not DifficultyModifier difficultyModifier)
            {
                return false;
            }
            return difficultyModifier.OneHealthMode == OneHealthMode 
                   && difficultyModifier.CheckPoints == CheckPoints 
                   && difficultyModifier.ZombieModifier == ZombieModifier
                   && difficultyModifier.HealingModifier == HealingModifier
                   && difficultyModifier.BossModifier == BossModifier;
            
        }
    }
}

