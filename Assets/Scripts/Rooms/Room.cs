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
    public class Room
    {
        public Room(RoomBounds bounds, Dictionary<TileMapLayer, TileBase[,]> layerTileDict, List<RoomDoor> doors, RoomObject roomObject) {
            this.bounds = bounds;
            this.layerTileDict = layerTileDict;
            this.doors = doors;
            if (roomObject != null) {
                this.ZombieSpawnInstructions = roomObject.GetZombieSpawnInstructions();
            } else {
                this.ZombieSpawnInstructions = new List<ZombieSpawnInstruction>();
            }
            
            this.roomObjects = roomObject;
            foreach (RoomDoor roomDoor in doors) {
                roomDoor.setRoom(this);
            }
        }
        private RoomBounds bounds;
        public RoomBounds Bounds => bounds;
        private Dictionary<TileMapLayer, TileBase[,]> layerTileDict;
        public Dictionary<TileMapLayer, TileBase[,]> LayerTileDict => layerTileDict;
        private List<RoomDoor> doors;
        public List<RoomDoor> RoomDoors => doors;
        public List<ZombieSpawnInstruction> ZombieSpawnInstructions;
        private Dictionary<GameObject, ZombieSpawnInstruction> spawnInstructions;
        private RoomObject roomObjects;
        public RoomObject RoomObjectContainer => roomObjects;
        public bool isClear() {
            return ZombieSpawnInstructions.Count==0;
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

