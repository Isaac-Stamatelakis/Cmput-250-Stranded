using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Linq;
using PlayerModule;

namespace Rooms {
    public class Level : MonoBehaviour
    {
        [SerializeField] private Tilemap floorTileMap;
        [SerializeField] private Tilemap wallTileMap;
        [SerializeField] private Tilemap doorTileMap;
        [SerializeField] private Transform roomElementContainer;
        [SerializeField] private LoadedRoom loadedRoomObject;
        [SerializeField] private RoomDoorObject roomDoorObjectPrefab;
        [SerializeField] private DatePlayer datePlayer;
        private Vector2Int spawnPosition;
        [SerializeField] private Transform spawnPositionObject;
        private List<Room> rooms = new List<Room>();
        private Dictionary<TileMapLayer, Tilemap> tileMapDict;
        private Dictionary<Vector2Int, RoomObject> positionRoomObjectCollection;
        private Room currentRoom;
        public Room CurrentRoom => currentRoom;
        public void Load() {
            Player.Instance.DatePlayer = datePlayer;
            tileMapDict = new Dictionary<TileMapLayer, Tilemap>();
            tileMapDict[TileMapLayer.Floor] = floorTileMap;
            tileMapDict[TileMapLayer.Wall] = wallTileMap;
            tileMapDict[TileMapLayer.Door] = doorTileMap;
            loadSpawnPosition();
            loadRoomElements();
            LevelData levelData = new LevelData(spawnPosition,tileMapDict,positionRoomObjectCollection);
            rooms = RoomUtils.generateRooms(levelData);
            connectRoomDoors();
            deactivateRoomElements();
            loadRoom(rooms[levelData.SpawnRoomIndex]);
        }

        public bool CurrentRoomClear() {
            return loadedRoomObject.isClear();
        }

        private bool doorsIntersectWall(RoomDoor a, RoomDoor b) {
            int distance = a.distanceFrom(b);
            LineDirection lineDirection = a.GetLineDirection();
            Vector2Int vector2Int = lineDirection == LineDirection.Vertical ?
                Vector2Int.left :
                Vector2Int.up;
            for (int d = 1; d < distance; d++) {
                
            }
            return true;
        }

        private void clearTileMaps() {
            foreach (var kvp in tileMapDict) {
                Tilemap tilemap = kvp.Value;
                BoundsInt boundsInt = tilemap.cellBounds;
                for (int x = boundsInt.xMin; x < boundsInt.xMax; x++) {
                    for (int y = boundsInt.yMin; y < boundsInt.yMax; y++) {
                        tilemap.SetTile(new Vector3Int(x,y,0),null);
                    }
                }
            }
        }

        public void loadRoom(Room room)
        {
            currentRoom = room;
            clearTileMaps();
            loadedRoomObject.reset();
            loadRoomTileMap(room);
            loadRoomDoors(room);
            loadRoomObject(room);
            loadedRoomObject.loadRoom(room);
            Camera.main.GetComponent<RoomCameraControl>().setBounds(room.Bounds);
        }

        private void loadRoomTileMap(Room room) {
            RoomBounds bounds = room.Bounds;
            foreach (var kvp in room.LayerTileDict) {
                Tilemap tilemap = tileMapDict[kvp.Key];
                TileBase[,] tiles = kvp.Value;
                for (int x = 0; x < tiles.GetLength(0); x++) {
                    for (int y = 0; y < tiles.GetLength(1); y++) {
                        tilemap.SetTile(new Vector3Int(x+bounds.XMin,y+bounds.YMin,0),tiles[x,y]);
                    }
                }
            }
        }

        private void loadRoomDoors(Room room) {
            foreach (RoomDoor roomDoor in room.RoomDoors) {
                if (roomDoor.Connection != null) {
                    RoomDoorObject roomDoorObject = GameObject.Instantiate(roomDoorObjectPrefab);
                    roomDoorObject.transform.SetParent(loadedRoomObject.doorContainer,false);
                    roomDoorObject.setRoomDoor(roomDoor);  
                }
            }
        }

        private void loadRoomObject(Room room) {
            if (room.RoomObjectContainer != null) {
                room.RoomObjectContainer.gameObject.SetActive(true);
            }
        }

        



        private void connectRoomDoors() {
            for (int i = 0; i < rooms.Count; i++) {
                foreach (RoomDoor roomDoor in rooms[i].RoomDoors) {
                    RoomDoor closetDoor = null;
                    int closestDistance = int.MaxValue;
                    for (int j = i+1; j < rooms.Count; j++) {
                        foreach (RoomDoor otherRoomDoor in rooms[j].RoomDoors) {
                            if (!roomDoor.isParallelTo(otherRoomDoor)) {
                                continue;
                            }
                            int distance = otherRoomDoor.distanceFrom(roomDoor);
                            if (distance < closestDistance) {
                                closestDistance = distance;
                                closetDoor = otherRoomDoor;
                            }
                        }
                        if (closetDoor != null) {
                            (Direction a, Direction b) = RoomUtils.getDoorDirection(roomDoor,closetDoor);
                            roomDoor.setConnection(closetDoor,a);
                            closetDoor.setConnection(roomDoor,b);
                        }
                        
                    }
                }
            }
        }

        private void connectRoomDoor(RoomDoor roomDoor) {
            int maxSearch = 100;
            int dist = 1;
            LineDirection lineDirection = roomDoor.GetLineDirection();
            bool terminatePositive = false;
            bool terminalNegative = false;
            while (dist < maxSearch && (!terminalNegative || !terminatePositive)) {
                Vector3Int vector = lineDirection == LineDirection.Vertical ? 
                    new Vector3Int(dist,0) :
                    new Vector3Int(0,dist);
                Vector3Int pStartVector = roomDoor.StartPosition+vector;
                Vector3Int pEndVector = roomDoor.EndPosition+vector;
                if (doorTileMap.GetTile(pStartVector) != null && doorTileMap.GetTile(pEndVector) != null) {

                }
                Vector3Int nStartVector = roomDoor.StartPosition-vector;
                Vector3Int nEndVector = roomDoor.EndPosition-vector;
            }
        }

        private void loadSpawnPosition() {
            spawnPosition = new Vector2Int((int)spawnPositionObject.transform.position.x,(int)spawnPositionObject.transform.position.y);
            Player player = Player.Instance;
            player.transform.position = new Vector3(spawnPositionObject.transform.position.x,spawnPositionObject.transform.position.y,player.transform.position.z);
        
            GameObject.Destroy(spawnPositionObject.gameObject);
        }

        private void loadRoomElements() {
            positionRoomObjectCollection = new Dictionary<Vector2Int, RoomObject>();
            for (int i = 0; i < roomElementContainer.childCount; i++) {
                Transform child = roomElementContainer.GetChild(i);
                RoomObject roomObject = child.GetComponent<RoomObject>();
                if (roomObject == null) {
                    continue;
                }
                Vector2Int position = new Vector2Int((int)child.position.x,(int)child.position.y);
                positionRoomObjectCollection[position] = roomObject;
            }
        }

        private void deactivateRoomElements() {
            foreach (RoomObject roomObject in positionRoomObjectCollection.Values) {
                roomObject.gameObject.SetActive(false);
            }
        }

        public void FixedUpdate() {
            if (!loadedRoomObject.Room.isClear() && loadedRoomObject.isClear()) {
                loadedRoomObject.setRoomClear();
            }
        }

    }

    public class LevelData {
        public Vector2Int SpawnPosition;
        public int SpawnRoomIndex;
        public Dictionary<TileMapLayer, Tilemap> TileMapDict;
        public Dictionary<Vector2Int, RoomObject> PositionRoomObjectCollection;

        public LevelData(Vector2Int spawnPosition, Dictionary<TileMapLayer, Tilemap> tileMapDict, Dictionary<Vector2Int, RoomObject> positionRoomObjectCollection)
        {
            SpawnPosition = spawnPosition;
            TileMapDict = tileMapDict;
            PositionRoomObjectCollection = positionRoomObjectCollection;
        }

        public TileBase GetTile(Vector3Int position, TileMapLayer tileMapLayer) {
            return TileMapDict[tileMapLayer].GetTile(position);
        }
    }

    public class RoomBounds {
        public int XMin;
        public int XMax;
        public int YMin;
        public int YMax;
        public Vector2Int Size => new Vector2Int(XMax-XMin+1,YMax-YMin+1);
        public RoomBounds(int xMin, int xMax, int yMin, int yMax)
        {
            XMin = xMin;
            XMax = xMax;
            YMin = yMin;
            YMax = yMax;
        }
        public RoomBounds(Vector2Int vector) {
            XMin = vector.x;
            XMax = vector.x;
            YMin = vector.y;
            YMax = vector.y;
        }
        public void expand(Vector2Int vector2Int) {
            if (vector2Int.x < XMin) {
                XMin = vector2Int.x;
            } else if (vector2Int.x > XMax) {
                XMax = vector2Int.x;
            }
            if (vector2Int.y < YMin) {
                YMin = vector2Int.y;
            } else if (vector2Int.y > YMax) {
                YMax = vector2Int.y;
            }
        }

        public void expand(int amount) {
            XMin-=amount;
            XMax+=amount;
            YMin-=amount;
            YMax+=amount;
        }
        public bool empty() {
            return XMin==XMax && YMin == YMax;
        }


        public override string ToString()
        {
            return $"[{XMin},{YMin}]-[{XMax},{YMax}]";
        }
    }
}

