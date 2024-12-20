using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;
using PlayerModule;

namespace Rooms {
    public class LoadedRoom : MonoBehaviour
    {
        public Transform enemyContainer;
        public Transform doorContainer;
        private Room room;
        public Room Room => room;
        public bool Locked = false;
        private Dictionary<GameObject, ZombieSpawnInstruction> spawnedEnemyInstructionDict;
        public void reset() {
            GlobalUtils.deleteChildren(enemyContainer);
            GlobalUtils.deleteChildren(doorContainer);
            if (room != null && room.RoomObjectContainer != null) {
                updateEnemySpawnInstructions();
                room.RoomObjectContainer.gameObject.SetActive(false);
            }
            
        }

        private void updateEnemySpawnInstructions() {
            List<ZombieSpawnInstruction> updatedSpawnInstructions = new List<ZombieSpawnInstruction>();
            for (int i = 0; i < enemyContainer.childCount; i++) {
                GameObject enemy = enemyContainer.GetChild(i).gameObject;
                if (spawnedEnemyInstructionDict.ContainsKey(enemy)) {
                    updatedSpawnInstructions.Add(spawnedEnemyInstructionDict[enemy]);
                }
                
            }
            room.ZombieSpawnInstructions = updatedSpawnInstructions;
        }

        public void setRoomClear() {
            room.ZombieSpawnInstructions = new List<ZombieSpawnInstruction>();
            Locked = false;
            for (int i = 0; i < doorContainer.childCount; i++) {
                RoomDoorObject roomDoorObject = doorContainer.GetChild(i).GetComponent<RoomDoorObject>();
                roomDoorObject.setClear(true);
            }
            IRoomClearListener[] clearActions = room.RoomObjectContainer.GetComponentsInChildren<IRoomClearListener>();
            foreach (IRoomClearListener clearListener in clearActions) {
                clearListener.trigger();
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
                Vector3 position = spawned.transform.position;
                position.z = 0;
                spawned.transform.position = position;
            }

            if (room.RoomObjectContainer.Boss != null) {
                
                
                EnemyHealth boss = room.RoomObjectContainer.Boss;
                
                boss.gameObject.SetActive(true);
                boss.transform.SetParent(enemyContainer);
                 for (int i = 0; i < doorContainer.childCount; i++) {
                    RoomDoorObject roomDoorObject = doorContainer.GetChild(i).GetComponent<RoomDoorObject>();
                    roomDoorObject.setLocked();
                }
       
            }
        }

        public bool isClear() {
            return enemyContainer.childCount==0;
        }
    }

    public interface IRoomClearListener {
        public void trigger();
    }
}

