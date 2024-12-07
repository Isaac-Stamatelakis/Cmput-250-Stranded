using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Difficulty {
    public enum DifficultyPresets
    {
        Easy,
        Normal,
        Hard,
        Insane,
        Impossible
    }

    public static class DifficultyPresetExtension
    {
        public static DifficultyModifier GetDifficultyModifier(this DifficultyPresets p)
        {
            switch (p)
            {
                case DifficultyPresets.Easy:
                    return new DifficultyModifier(false, true, 0, 3, 0);
                case DifficultyPresets.Normal:
                    return new DifficultyModifier(false, true, 1, 2, 1);
                case DifficultyPresets.Hard:
                    return new DifficultyModifier(false, true, 2, 1, 2);
                case DifficultyPresets.Insane:
                    return new DifficultyModifier(false, false, 3, 0, 3);
                case DifficultyPresets.Impossible:
                    return new DifficultyModifier(true, false, 3, 0, 3);
                default:
                    throw new InvalidEnumArgumentException($"Did not cover case for {p}");
            }
        }
    }
    public static class DifficultyUtils
    {
        public static readonly List<float> ZombieModifiers = new List<float> { 0.5f, 1f, 1.5f,2f};
        public static readonly List<float> HealingModifiers = new List<float> { 0, 0.5f, 1f, 1.5f };
        public static readonly List<float> BossHealthModifier = new List<float> { 0.5f, 1f, 1.5f,2f};
        public static readonly List<float> BossSpeedModifier = new List<float> { 1.5f, 1f, 0.5f,0}; // Good luck
        public static readonly int DEFAULT_ZOMBIE_MODIFIER = 1;
        public static readonly int DEFAULT_HEALING_MODIFIER = 2;
        public static readonly int DEFAULT_BOSS_MODIFIER = 1;
    }
}

