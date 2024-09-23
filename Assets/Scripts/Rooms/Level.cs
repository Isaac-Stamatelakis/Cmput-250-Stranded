using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rooms {
    public class Level : MonoBehaviour
    {
        private Room currentRoom;
        public void Start() {
            Room[] rooms = GetComponentsInChildren<Room>();
            List<RoomNode> nodes = new List<RoomNode>();
            foreach (Room room in rooms) {
                Debug.Log($"{room.name} {room.getMidpoint()}");
                List<RoomDoor> doors = room.generateDoors();
                foreach (RoomDoor roomDoor in doors) {
                    Debug.Log($"{room.name} {roomDoor}");
                }
                RoomNode roomNode = new RoomNode(room,doors);
                nodes.Add(roomNode);
            }
            for (int i = 0; i < nodes.Count; i++) {
                Vector3Int currentMidpoint = nodes[i].Room.getMidpoint();
                foreach (RoomDoor roomDoor in nodes[i].Connections) {
                    int smallestDistance = int.MaxValue;
                    RoomDoor closestConnection = null;
                    for (int j = i+1; j < nodes.Count; j++) {
                        Vector3Int otherMidpoint = nodes[j].Room.getMidpoint();
                        foreach (RoomDoor otherRoomDoor in nodes[j].Connections) {
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

    public class RoomNode {
        private Room room;
        public Room Room => room;
        private List<RoomDoor> connections;
        public List<RoomDoor> Connections => connections;

        public RoomNode(Room room, List<RoomDoor> connections)
        {
            this.room = room;
            this.connections = connections;
        }
    }

    public class AdjacentRoomDoor {
        private Room adjcaentRoom;
        private RoomDoor roomDoor;

        public AdjacentRoomDoor(Room adjcaentRoom, RoomDoor roomDoor)
        {
            this.adjcaentRoom = adjcaentRoom;
            this.roomDoor = roomDoor;
        }
    }
}

