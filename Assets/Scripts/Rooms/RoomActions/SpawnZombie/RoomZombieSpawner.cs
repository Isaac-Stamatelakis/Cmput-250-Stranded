using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rooms {
    public class RoomZombieSpawner : RoomElement
    {
        [SerializeField] private GameObject zombie;
        public int SpawnAmount;
        public int SpawnRange;

        public List<ZombieSpawnInstruction> GetSpawnInstructions(float modifier) {
            List<ZombieSpawnInstruction> spawnInstructions = new List<ZombieSpawnInstruction>();
            SpawnRange = Mathf.Clamp(SpawnRange, 1, int.MaxValue);
            float fSpawnRange = (float) SpawnRange;
            for (int i = 0; i < Mathf.CeilToInt(modifier * SpawnAmount); i++) {
                float ranx = UnityEngine.Random.Range(-fSpawnRange,fSpawnRange);
                float rany = UnityEngine.Random.Range(-fSpawnRange,fSpawnRange);
                Vector2 randOffset = new Vector2(ranx,rany);
                spawnInstructions.Add(new ZombieSpawnInstruction(randOffset+(Vector2)transform.position,zombie));
            }
            return spawnInstructions;
        }
    }
}

