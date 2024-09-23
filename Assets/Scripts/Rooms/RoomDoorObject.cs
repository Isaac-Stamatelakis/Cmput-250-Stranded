using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rooms {
    public class RoomDoorObject : MonoBehaviour
    {
        private RoomDoor roomDoor;
        public void setRoomDoor(RoomDoor roomDoor) {
            this.roomDoor = roomDoor;
        }
        public void switchRoom(Transform playerTransform) {
            Room room = GetComponentInParent<Room>();
            room.unload();
            Debug.Log(roomDoor.Direction);
            roomDoor.Connection.Room.load();
            Vector3 spawnPosition = roomDoor.Connection.getEnterPosition(playerTransform.position);
            playerTransform.position = spawnPosition;

        }
    }
}

