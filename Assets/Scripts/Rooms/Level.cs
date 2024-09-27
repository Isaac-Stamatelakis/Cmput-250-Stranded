using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rooms {
    public class Level : MonoBehaviour
    {
        private Room currentRoom;
        public void Start() {
            Room[] rooms = GetComponentsInChildren<Room>();
            foreach (Room room in rooms) {
                room.generateDoors();
            }
            for (int i = 0; i < rooms.Length; i++) {
                foreach (RoomDoor roomDoor in rooms[i].RoomDoors) {
                    int smallestDistance = int.MaxValue;
                    RoomDoor closestConnection = null;
                    for (int j = i+1; j < rooms.Length; j++) {
                        foreach (RoomDoor otherRoomDoor in rooms[j].RoomDoors) {
                            if (!roomDoor.isParallelTo(otherRoomDoor)) {
                                continue;
                            }
                            int distance = roomDoor.distanceFrom(otherRoomDoor);
                            if (Mathf.Abs(distance) < smallestDistance) {
                                smallestDistance = distance;
                                closestConnection = otherRoomDoor;
                            }
                        }
                    }
                    if (closestConnection != null) {
                        (Direction a, Direction b) = RoomUtils.getDoorDirection(roomDoor,closestConnection);
                        roomDoor.setConnection(closestConnection,a);
                        closestConnection.setConnection(roomDoor,b);
                    }
                }
            }

            for (int i = 1; i < rooms.Length; i++) {
                rooms[i].gameObject.SetActive(false);
            }
            currentRoom = rooms[0];
            currentRoom.load();
            
        }

        public void FixedUpdate() {
            if (currentRoom.isClear()) {

            }
        }

    }
}

