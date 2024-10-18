using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;
using PlayerModule;

namespace Rooms {
    public class LoadedRoom : MonoBehaviour
    {
        public Transform enemyContainer;
        public Transform triggerContainer;
        public Transform interactableContainer;
        public Transform doorContainer;
        private Room room;
        public Room Room => room;
        private Dictionary<GameObject, ZombieSpawnInstruction> spawnedEnemyInstructionDict;
        public void reset() {
            GlobalUtils.deleteChildren(enemyContainer);
            GlobalUtils.deleteChildren(triggerContainer);
            GlobalUtils.deleteChildren(interactableContainer);
            GlobalUtils.deleteChildren(doorContainer);
            if (room != null && room.RoomObjectContainer != null) {
                updateEnemySpawnInstructions();
                room.RoomObjectContainer.gameObject.SetActive(false);
            }
            
        }

        private void updateEnemySpawnInstructions() {
            List<ZombieSpawnInstruction> updatedSpawnInstructions = new List<ZombieSpawnInstruction>();
            for (int i = 0; i < enemyContainer.childCount; i++) {
                updatedSpawnInstructions.Add(spawnedEnemyInstructionDict[enemyContainer.GetChild(i).gameObject]);
            }
            room.ZombieSpawnInstructions = updatedSpawnInstructions;
        }

        public void setRoomClear() {
            room.ZombieSpawnInstructions = new List<ZombieSpawnInstruction>();
            for (int i = 0; i < doorContainer.childCount; i++) {
                RoomDoorObject roomDoorObject = doorContainer.GetChild(i).GetComponent<RoomDoorObject>();
                roomDoorObject.setClear(true);
            }
            if (room.RoomObjectContainer.OnRoomClearDialog != null) {
                DialogUIController.Instance.DisplayDialogue(room.RoomObjectContainer.OnRoomClearDialog);
            }
        }

        public void loadRoom(Room room) {
            if (room.RoomObjectContainer.DateRandomDialogs != null) {
                Player.Instance.DatePlayer.SetRandomDialogues(room.RoomObjectContainer.DateRandomDialogs.dialogs);
            }
            this.room = room;
            loadRoomEnemies(room);
        }

        private void loadRoomEnemies(Room room) {
            spawnedEnemyInstructionDict = new Dictionary<GameObject, ZombieSpawnInstruction>();
            foreach (ZombieSpawnInstruction zombieSpawnInstruction in room.ZombieSpawnInstructions) {
                GameObject spawned = GameObject.Instantiate(zombieSpawnInstruction.prefab);
                spawned.transform.position = zombieSpawnInstruction.spawnPosition;
                spawnedEnemyInstructionDict[spawned] = zombieSpawnInstruction;
                spawned.gameObject.SetActive(true);
                spawned.transform.SetParent(enemyContainer,false);
            }
        }

        public bool isClear() {
            return enemyContainer.childCount==0;
        }
    }
}

