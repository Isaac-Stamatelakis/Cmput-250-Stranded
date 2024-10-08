using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    namespace Rooms {
    public class ZombieSpawnInstruction
    {
        public Vector2 spawnPosition;
        public GameObject prefab;

        public ZombieSpawnInstruction(Vector2 spawnPosition, GameObject prefab)
        {
            this.spawnPosition = spawnPosition;
            this.prefab = prefab;
        }
    }
}

