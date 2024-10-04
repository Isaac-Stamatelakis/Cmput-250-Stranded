using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;

namespace Rooms {
    public class RoomDoorObject : MonoBehaviour
    {
        private RoomDoor roomDoor;
        public void setRoomDoor(RoomDoor roomDoor) {
            this.roomDoor = roomDoor;
        }
        public void switchRoom(Transform playerTransform) {
            Room room = roomDoor.Room;
            Level.Instance.loadRoom(roomDoor.Connection.Room);
            Vector3 spawnPosition = roomDoor.Connection.getEnterPosition(playerTransform.position);
            Player.Instance.SetPosition(spawnPosition);
        }
    }
}

