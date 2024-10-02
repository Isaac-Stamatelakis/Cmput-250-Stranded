using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Rooms {
    
    public enum TileMapLayer {
        Floor,
        Wall,
        Door
    }
    public interface IRoom {
        public void load();
        public void unload();
    }
    public class Room
    {
        public Room(RoomBounds bounds, Dictionary<TileMapLayer, TileBase[,]> layerTileDict, List<RoomDoor> doors) {
            this.bounds = bounds;
            this.layerTileDict = layerTileDict;
            this.doors = doors;
            foreach (RoomDoor roomDoor in doors) {
                roomDoor.setRoom(this);
            }
        }
        private RoomBounds bounds;
        public RoomBounds Bounds => bounds;
        private Dictionary<TileMapLayer, TileBase[,]> layerTileDict;
        private List<RoomDoor> doors;
        public List<RoomDoor> RoomDoors => doors;
        public void load(Dictionary<TileMapLayer, Tilemap> tileMapDict, LoadedRoom loadedRoom) {
            activeDoors(loadedRoom.doorContainer);
            foreach (var kvp in layerTileDict) {
                Tilemap tilemap = tileMapDict[kvp.Key];
                TileBase[,] tiles = kvp.Value;
                for (int x = 0; x < tiles.GetLength(0); x++) {
                    for (int y = 0; y < tiles.GetLength(1); y++) {
                        tilemap.SetTile(new Vector3Int(x+bounds.XMin,y+bounds.YMin,0),tiles[x,y]);
                    }
                }
            }
        }
        public void unload() {

        }
        /*
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
        */


        public void activeDoors(Transform container) {
            foreach (RoomDoor roomDoor in doors) {
                if (roomDoor.Connection != null && roomDoor.Connection.Room.isClear()) {
                    RoomDoorObject roomDoorObject = RoomUtils.createRoomDoorObject(roomDoor);
                    roomDoorObject.transform.SetParent(container,false);
                }
            }
        }
        public bool isClear() {
            return true;
        }


        public void load()
        {
            
        }
        /*
        public void unload()
        {
            for (int i = 0; i < roomDoorContainer.childCount; i++) {
                GameObject.Destroy(roomDoorContainer.GetChild(i).gameObject);
            }
            gameObject.SetActive(false);
        }
        */
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

