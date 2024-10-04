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
        private Vector2Int spawnPosition;
        private int spawnRoomIndex = -1;
        [SerializeField] private Transform spawnPositionObject;
        private List<Room> rooms = new List<Room>();
        private Dictionary<TileMapLayer, Tilemap> tileMapDict;
        private Dictionary<Vector2Int, Transform> positionRoomElementCollectionDict; 
        private static Level instance;
        public static Level Instance => instance;
        public void Awake() {
            instance = this;
        }
        public void Start() {
            tileMapDict = new Dictionary<TileMapLayer, Tilemap>();
            tileMapDict[TileMapLayer.Floor] = floorTileMap;
            tileMapDict[TileMapLayer.Wall] = wallTileMap;
            tileMapDict[TileMapLayer.Door] = doorTileMap;
            loadSpawnPosition();
            loadRoomElements();
            generateRooms();
            connectRoomDoors();
            loadRoom(rooms[spawnRoomIndex]);
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

        public void loadRoom(Room room) {
            clearTileMaps();
            loadedRoomObject.reset();
            loadedRoomObject.setRoom(room);
            room.load(tileMapDict,loadedRoomObject);
            Camera.main.GetComponent<RoomCameraControl>().setBounds(room.Bounds);
        }

        private void connectRoomDoors() {
            for (int i = 0; i < rooms.Count; i++) {
                foreach (RoomDoor roomDoor in rooms[i].RoomDoors) {
                    for (int j = i+1; j < rooms.Count; j++) {
                        foreach (RoomDoor otherRoomDoor in rooms[j].RoomDoors) {
                            if (!roomDoor.isParallelTo(otherRoomDoor)) {
                                continue;
                            }
                            (Direction a, Direction b) = RoomUtils.getDoorDirection(roomDoor,otherRoomDoor);
                            roomDoor.setConnection(otherRoomDoor,a);
                            otherRoomDoor.setConnection(roomDoor,b);
                            break;
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
            positionRoomElementCollectionDict = new Dictionary<Vector2Int, Transform>();
            for (int i = 0; i < roomElementContainer.childCount; i++) {
                Transform child = roomElementContainer.GetChild(i);
                Vector2Int position = new Vector2Int((int)child.position.x,(int)child.position.y);
                RoomElement[] roomElements = child.GetComponentsInChildren<RoomElement>();
                child.GetComponent<MeshRenderer>().enabled = false;
                positionRoomElementCollectionDict[position] = child;
                child.gameObject.SetActive(false);
            }
        }

        private void generateRooms() {
            BoundsInt wallBounds = wallTileMap.cellBounds;
            BoundsInt doorBounds = doorTileMap.cellBounds;
            Vector3Int size = wallBounds.size;
            bool[,] seen = new bool[size.x,size.y];
            Vector2Int[] directions = {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.left,
                Vector2Int.right
            };
            for (int x = wallBounds.xMin; x < wallBounds.xMax; x++) {
                for (int y = wallBounds.yMin; y < wallBounds.yMax; y++) {
                    int xIndexed = x - wallBounds.xMin;
                    int yIndexed = y - wallBounds.yMin;
                    if (seen[xIndexed,yIndexed]) {
                        continue;
                    }
                    FloodFill(new Vector2Int(x,y),seen,wallBounds, directions);
                }
            }
        }

        private void FloodFill(Vector2Int start, bool[,] seen, BoundsInt bounds, Vector2Int[] directions) {
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            queue.Enqueue(start);
            RoomBounds roomBounds = new RoomBounds(start);
            Dictionary<Vector2Int, List<TileBase>> positionTileDict = new Dictionary<Vector2Int, List<TileBase>>();
            Transform roomElementCollection = null;
            while (queue.Count > 0) {
                Vector2Int current = queue.Dequeue();
                int x = current.x - bounds.xMin;
                int y = current.y - bounds.yMin;
                if (x < 0 || x >= seen.GetLength(0) || y < 0 || y >= seen.GetLength(1)) {
                    continue;
                }
                Vector3Int vector = (Vector3Int) current;
                positionTileDict[current] = getTileList(vector);
                if (current == spawnPosition) {
                    spawnRoomIndex = rooms.Count;
                }
                if (seen[x,y]) {
                    continue;
                }
                if (positionRoomElementCollectionDict.ContainsKey(current)) {
                    roomElementCollection = positionRoomElementCollectionDict[current];
                }
                seen[x,y] = true;
                if (wallTileMap.GetTile(vector) == null && doorTileMap.GetTile(vector) == null) {
                    roomBounds.expand(current);
                    foreach (Vector2Int direction in directions)
                    {
                        Vector2Int neighbor = current + direction;
                        queue.Enqueue(neighbor);
                    }
                } 
            }
            if (!roomBounds.empty()) {
                bool enclosed = roomBounds.XMax != bounds.xMax && roomBounds.XMin != bounds.xMin && roomBounds.YMax != bounds.yMax && roomBounds.YMin != bounds.yMin;
                if (!enclosed) {
                    if (spawnRoomIndex != -1) {
                        Debug.LogWarning("PLAYER SPAWN POSITION OUT OF BOUNDS");
                    }
                    return;
                }
                roomBounds.expand(1); // Expand to include walls & doors
                HashSet<Vector2Int> seenBorder = new HashSet<Vector2Int>();
                List<RoomDoor> roomDoors = new List<RoomDoor>();
                // Depth first search the border of the room to find any gaps and doors
                // Has to be a wall on the border which is adjacent to a door
                foreach (var kvp in positionTileDict) {
                    bool hasWall = kvp.Value[1] != null;
                    if (!hasWall) {
                        continue;
                    }
                    bool hasAdjcaentDoor = false;
                    foreach (Vector2Int directionVector in directions) {
                        Vector2Int pos = kvp.Key + directionVector;
                        if (doorTileMap.GetTile((Vector3Int)pos) != null) {
                            hasAdjcaentDoor = true;
                            break;
                        }
                    }
                    if (!hasAdjcaentDoor) {
                        continue;
                    }
                    DFSBorder(kvp.Key,roomBounds,null,seenBorder, positionTileDict, roomDoors);
                    break;
                    
                }
                Dictionary<TileMapLayer, TileBase[,]> layerTileDict = new Dictionary<TileMapLayer, TileBase[,]>();
                List<TileMapLayer> layers = Enum.GetValues(typeof(TileMapLayer)).Cast<TileMapLayer>().ToList();
                Vector2Int size = roomBounds.Size;
                foreach (TileMapLayer layer in layers) {
                    layerTileDict[layer] = new TileBase[size.x,size.y];
                }
                foreach (var kvp in positionTileDict) {
                    int xIndex = kvp.Key.x - roomBounds.XMin;
                    int yIndex = kvp.Key.y - roomBounds.YMin;      
                    for (int i = 0; i < layers.Count; i++) {
                        TileMapLayer layer = layers[i];
                        TileBase tile = kvp.Value[i];
                        layerTileDict[layer][xIndex,yIndex] = tile;
                    }
                }
                rooms.Add(new Room(roomBounds,layerTileDict,roomDoors,roomElementCollection));
                
            }
        }

        private void DFSBorder(
            Vector2Int current, 
            RoomBounds roomBounds, 
            Direction? originDirection, 
            HashSet<Vector2Int> seenBorder, 
            Dictionary<Vector2Int, List<TileBase>> positionTileDict,
            List<RoomDoor> roomDoors
            ) {
            if (current.x < roomBounds.XMin || current.x > roomBounds.XMax || current.y < roomBounds.YMin || current.y > roomBounds.YMax || seenBorder.Contains(current)) {
                return;
            }
            seenBorder.Add(current);
            Vector3Int vector = (Vector3Int) current;
            bool wallTile = wallTileMap.GetTile(vector) != null;
            bool doorTile = doorTileMap.GetTile(vector) != null;
            if (!wallTile && !doorTile) {
                return;
            }
            if (doorTile) {
                if (roomDoors.Count == 0) {
                    roomDoors.Add(new RoomDoor(vector));
                } else {
                    RoomDoor roomDoor = roomDoors.Last();
                    if (roomDoor.adjacentPointOnLine(vector)) {
                        roomDoor.expand(vector);
                    } else {
                        roomDoors.Add(new RoomDoor(vector));
                    }
                }
            }
            // Flood fill will not fill corners of the room
            if (!positionTileDict.ContainsKey(current)) {
                positionTileDict[current] = getTileList(vector);
            }
            // Don't search in a direction we've came from
            if (originDirection != Direction.Right) {
                DFSBorder(current+Vector2Int.left,roomBounds,Direction.Left,seenBorder, positionTileDict, roomDoors);
            }
            if (originDirection != Direction.Left) {
                DFSBorder(current+Vector2Int.right,roomBounds,Direction.Right,seenBorder, positionTileDict, roomDoors);
            }
            if (originDirection != Direction.Down) {
                DFSBorder(current+Vector2Int.up,roomBounds,Direction.Up,seenBorder, positionTileDict, roomDoors);
            }
            if (originDirection != Direction.Up) {
                DFSBorder(current+Vector2Int.down,roomBounds,Direction.Down,seenBorder, positionTileDict, roomDoors);
            }

        }

        private List<TileBase> getTileList(Vector3Int vector) {
            return new List<TileBase>{floorTileMap.GetTile(vector),wallTileMap.GetTile(vector),doorTileMap.GetTile(vector)};
        }

        

        public void FixedUpdate() {
            
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

