using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace TitleScreen {
    public static class ZombiePatternFactory
    {
        public static TitleScreenZombiePattern createPattern(ZombiePattern zombiePattern, Vector2Int direction, Vector2 position, TitleScreenZombie prefab) {
            switch (zombiePattern) {
                case ZombiePattern.Single:
                    return new SingleZombiePattern(direction,position,prefab);
                case ZombiePattern.Line:
                    return new LineZombiePattern(direction,position,prefab);
                case ZombiePattern.Arrow:
                    return new ArrowZombiePattern(direction,position,prefab);
                case ZombiePattern.Circle:
                    return new CirlceZombiePattern(direction,position,prefab);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns a random zombie pattern with weights defined in ZombiePatternExtension
        /// </summary>
        public static ZombiePattern getRandomType() {
            List<ZombiePattern> patterns = Enum.GetValues(typeof(ZombiePattern)).Cast<ZombiePattern>().ToList();
            List<(ZombiePattern, int)> weightPatternList = new List<(ZombiePattern, int)>();
            int totalWeights = 0;
            foreach (ZombiePattern zombiePattern in patterns) {
                int weight = zombiePattern.getWeight();
                weightPatternList.Add((zombiePattern,weight));
                totalWeights += weight;
            }
            int ran = UnityEngine.Random.Range(0,totalWeights);
            totalWeights = 0;
            foreach ((ZombiePattern, int) patternWeight in weightPatternList) {
                totalWeights += patternWeight.Item2;
                if (ran < totalWeights) {
                    return patternWeight.Item1;
                }
            }
            Debug.LogWarning("Something went wrong in pattern factory");
            return ZombiePattern.Single;
        }
    }

    public static class ZombiePatternExtension {
        public static int getWeight(this ZombiePattern zombiePattern) {
            switch (zombiePattern) {
                case ZombiePattern.Single:
                    return 5;
                case ZombiePattern.Arrow:
                    return 2;
                case ZombiePattern.Line:
                    return 2;
                case ZombiePattern.Circle:
                    return 1;
                default:
                    return 1;
            }
        }
    }
}

