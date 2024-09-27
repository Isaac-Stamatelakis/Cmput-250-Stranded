using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Rooms {

    public interface IRoom {
        public void load();
        public void unload();
    }
    public class Room : MonoBehaviour, IRoom
    {
        [SerializeField] private Tilemap wallTileMap;
        [SerializeField] private Tilemap doorTileMap;
        [SerializeField] private Transform roomDoorContainer;
        private List<RoomDoor> roomDoors;
        public List<RoomDoor> RoomDoors => roomDoors;
        
        public List<RoomDoor> generateDoors() {
            roomDoors = new List<RoomDoor>();
            BoundsInt doorBounds = wallTileMap.cellBounds;
            bool[,] visited = new bool[doorBounds.size.x, doorBounds.size.y];
            Vector3Int offset = new Vector3Int(doorBounds.xMin,doorBounds.yMin);
            for (int x = doorBounds.xMin; x < doorBounds.xMax; x++)
            {
                for (int y = doorBounds.yMin; y < doorBounds.yMax; y++)
                {
                    int xIndex = x-doorBounds.xMin;
                    int yIndex = y-doorBounds.yMin;
                    if (visited[xIndex, yIndex]) {
                        continue;
                    }
                    Vector3Int vector3 = new Vector3Int(x,y,0);
                    TileBase tileBase = doorTileMap.GetTile(vector3);
                    if (tileBase == null) {
                        continue;
                    }
                    RoomDoor roomDoor = new RoomDoor(vector3,this);
                    visited[xIndex,yIndex] = true;
                    RoomUtils.expandRoomDoor(roomDoor,vector3,offset,visited,doorTileMap);
                    roomDoors.Add(roomDoor);
                    
                }
            }    
            return roomDoors;
        }

        public Vector3Int getMidpoint() {
            BoundsInt boundsInt = wallTileMap.cellBounds;
            return new Vector3Int((boundsInt.xMax+boundsInt.xMin)/2,(boundsInt.yMax+boundsInt.yMin)/2);
        }


        public void activeDoors() {
            foreach (RoomDoor roomDoor in roomDoors) {
                if (roomDoor.Connection != null && roomDoor.Connection.Room.isClear()) {
                    RoomDoorObject roomDoorObject = RoomUtils.createRoomDoorObject(roomDoor);
                    roomDoorObject.transform.SetParent(roomDoorContainer,false);
                }
            }
        }
        public bool isClear() {
            return true;
        }


        public void load()
        {
            gameObject.SetActive(true);
            activeDoors();
        }

        public void unload()
        {
            for (int i = 0; i < roomDoorContainer.childCount; i++) {
                GameObject.Destroy(roomDoorContainer.GetChild(i).gameObject);
            }
            gameObject.SetActive(false);
            
        }
    }

    public enum LineDirection {
        Vertical,
        Horizontal
    }

    public enum Direction {
        Right,
        Left,
        Up,
        Down
    }
}

