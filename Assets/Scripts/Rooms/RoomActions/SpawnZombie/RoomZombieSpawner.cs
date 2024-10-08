using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rooms {
    public class RoomZombieSpawner : RoomElement
    {
        [SerializeField] private GameObject zombie;
        public int SpawnAmount;
        public int SpawnRange;

        public List<ZombieSpawnInstruction> GetSpawnInstructions() {
            List<ZombieSpawnInstruction> spawnInstructions = new List<ZombieSpawnInstruction>();
            for (int i = 0; i < SpawnAmount; i++) {
                float angle = UnityEngine.Random.Range(0,2*Mathf.PI);
                float r = UnityEngine.Random.Range(0,SpawnRange);
                Vector2 circlePosition = r * new Vector2(Mathf.Sin(angle),Mathf.Cos(angle));
                spawnInstructions.Add(new ZombieSpawnInstruction(circlePosition+(Vector2)transform.position,zombie));
            }
            return spawnInstructions;
        }
    }
}

