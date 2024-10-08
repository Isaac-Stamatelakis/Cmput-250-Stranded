using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rooms {
    public class RoomObject : MonoBehaviour
    {
        [SerializeField] private MeshRenderer textIndicator;

        public void Start() {
            if (textIndicator != null && textIndicator.gameObject != gameObject) {
                textIndicator.gameObject.SetActive(false);
            }
        }
        public List<ZombieSpawnInstruction> GetZombieSpawnInstructions() {
            List<ZombieSpawnInstruction> spawnInstructions = new List<ZombieSpawnInstruction>();
            for (int i = 0; i < transform.childCount; i++) {
                Transform child = transform.GetChild(i);
                Vector2Int position = new Vector2Int((int)child.position.x,(int)child.position.y);
                RoomElement[] roomElements = child.GetComponentsInChildren<RoomElement>();
                RoomZombieSpawner roomZombieSpawner = child.GetComponent<RoomZombieSpawner>();
                if (roomZombieSpawner != null) {
                    spawnInstructions.AddRange(roomZombieSpawner.GetSpawnInstructions());
                    continue;
                }
                // This method is not recommended to use, use room zombie spawners instead
                // TODO: Change this to an enemy script
                EnemyHealth enemyHealth = child.GetComponent<EnemyHealth>();
                if (enemyHealth != null) {
                    ZombieSpawnInstruction zombieSpawnInstruction = new ZombieSpawnInstruction(enemyHealth.transform.position,enemyHealth.gameObject);
                    spawnInstructions.Add(zombieSpawnInstruction);
                    enemyHealth.gameObject.SetActive(false);
                }
            }
            return spawnInstructions;
        }
    }
}

