using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;
using Difficulty;

namespace Rooms {
    public class RoomObject : MonoBehaviour
    {
        [SerializeField] private MeshRenderer textIndicator;
        [SerializeField] private EnemyHealth boss;
        public EnemyHealth Boss => boss;
        public DialogCollection DateRandomDialogs;
        public DialogObject OnRoomClearDialog;

        public void Start() {
            if (textIndicator != null) {
                bool isThis = textIndicator.gameObject == gameObject;
                if (isThis) {
                    textIndicator.enabled = false;
                } else {
                    textIndicator.gameObject.SetActive(false);
                }
            }
        }
        public List<ZombieSpawnInstruction> GetZombieSpawnInstructions() {
            DifficultyModifier modifier = LevelManager.getInstance().DifficultyModifier;
            float spawnModifier = modifier.GetZombieModifier();
            List<ZombieSpawnInstruction> spawnInstructions = new List<ZombieSpawnInstruction>();
            for (int i = 0; i < transform.childCount; i++) {
                Transform child = transform.GetChild(i);
                Vector2Int position = new Vector2Int((int)child.position.x,(int)child.position.y);
                RoomElement[] roomElements = child.GetComponentsInChildren<RoomElement>();
                RoomZombieSpawner roomZombieSpawner = child.GetComponent<RoomZombieSpawner>();
                if (roomZombieSpawner != null) {
                    spawnInstructions.AddRange(roomZombieSpawner.GetSpawnInstructions(spawnModifier));
                    GameObject.Destroy(roomZombieSpawner.gameObject);
                    continue;
                }
            }
            return spawnInstructions;
        }
    }
}

