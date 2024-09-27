using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Rooms {
    public static class RoomUtils
    {
        public static void expandRoomDoor(RoomDoor roomDoor, Vector3Int position, Vector3Int offset, bool[,] visited, Tilemap doorMap) {
            searchVector(roomDoor, position+Vector3Int.left, offset, visited,doorMap);
            searchVector(roomDoor, position+Vector3Int.right, offset, visited,doorMap);
            searchVector(roomDoor, position+Vector3Int.up, offset, visited,doorMap);
            searchVector(roomDoor, position+Vector3Int.down, offset, visited,doorMap);
        }

        private static void searchVector(RoomDoor roomDoor, Vector3Int vector, Vector3Int offset, bool[,] visited, Tilemap doorMap) {
            Vector3Int positionInRoom = vector - offset;
            if (   positionInRoom.x < 0 
                || positionInRoom.x >= visited.GetLength(0)
                || positionInRoom.y < 0
                || positionInRoom.y >= visited.GetLength(1)
                || visited[positionInRoom.x,positionInRoom.y]
                || doorMap.GetTile(vector) == null
            ) {
                return;
            }
            if (doorMap.GetTile(vector) != null) {
                roomDoor.expand(vector);
                visited[positionInRoom.x,positionInRoom.y] = true;
                expandRoomDoor(roomDoor,vector,offset,visited,doorMap);
            }
        }

        public static bool doorCanConnect(LineDirection lineDirection, Vector3Int midpoint, Vector3Int adjcanetMidpoint, int distance) {
            bool positiveDistance = distance >= 0;
            if (positiveDistance) {
                if (lineDirection == LineDirection.Vertical) {
                    return midpoint.x < adjcanetMidpoint.x;
                }
                return midpoint.x < adjcanetMidpoint.x;
            } else {
                if (lineDirection == LineDirection.Vertical) {
                    return midpoint.y > adjcanetMidpoint.y;
                }
                return midpoint.y > adjcanetMidpoint.y;
            }
        }

        public static (Direction, Direction) getDoorDirection(RoomDoor a, RoomDoor b) {
            LineDirection lineDirection = a.getDirection();
            if (lineDirection == LineDirection.Vertical) {
                if (a.StartPosition.x > b.StartPosition.x) {
                    return (Direction.Left,Direction.Right);
                } else {
                    return (Direction.Right,Direction.Left);
                }
            } else {
                if (a.StartPosition.y > b.StartPosition.y) {
                    return (Direction.Up,Direction.Down);
                } else {
                    return (Direction.Down,Direction.Up);
                }
            }
        }

        public static RoomDoorObject createRoomDoorObject(RoomDoor roomDoor) {
            GameObject gameObject = new GameObject();
            gameObject.name = $"Connection to {roomDoor.Connection.Room.name}";
            BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            boxCollider2D.gameObject.layer = LayerMask.NameToLayer(GlobalUtils.WALL_LAYER_NAME);
            LineDirection lineDirection = roomDoor.getDirection();
            Vector2 size = new Vector3(1.25f,roomDoor.Size) * GlobalUtils.TILE_SIZE;
            if (lineDirection == LineDirection.Horizontal) {
                boxCollider2D.size = new Vector2(size.y,size.x);
            } else {
                boxCollider2D.size = size;
            }
            RoomDoorObject roomDoorObject = gameObject.AddComponent<RoomDoorObject>();
            roomDoorObject.transform.position = roomDoor.getMidpoint()+new Vector3(0.5f,0.5f);
            roomDoorObject.setRoomDoor(roomDoor);
            return roomDoorObject;
        }
    }
}


