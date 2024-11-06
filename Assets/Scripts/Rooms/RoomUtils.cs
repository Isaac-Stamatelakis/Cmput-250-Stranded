using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;
using System;

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
            LineDirection lineDirection = a.GetLineDirection();
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
            //gameObject.name = $"Connection to {roomDoor.Connection.Room.name}";
            gameObject.name = "Connection";
            BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            boxCollider2D.gameObject.layer = LayerMask.NameToLayer(GlobalUtils.WALL_LAYER_NAME);
            LineDirection lineDirection = roomDoor.GetLineDirection();
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

        public static List<Room> generateRooms(LevelData levelData) {
            BoundsInt wallBounds = levelData.TileMapDict[TileMapLayer.Wall].cellBounds;
            BoundsInt doorBounds = levelData.TileMapDict[TileMapLayer.Door].cellBounds;
            Vector3Int size = wallBounds.size;
            bool[,] seen = new bool[size.x,size.y];
            Vector2Int[] directions = {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.left,
                Vector2Int.right
            };
            List<Room> rooms = new List<Room>();
            for (int x = wallBounds.xMin; x < wallBounds.xMax; x++) {
                for (int y = wallBounds.yMin; y < wallBounds.yMax; y++) {
                    int xIndexed = x - wallBounds.xMin;
                    int yIndexed = y - wallBounds.yMin;
                    if (seen[xIndexed,yIndexed]) {
                        continue;
                    }
                    FloodFill(new Vector2Int(x,y),seen,wallBounds, directions, levelData, rooms);
                }
            }
            Debug.Log($"Generated {rooms.Count} Rooms");
            return rooms;
        }

        private static void FloodFill(
            Vector2Int start, 
            bool[,] seen, 
            BoundsInt bounds, 
            Vector2Int[] directions,  
            LevelData levelData, 
            List<Room> rooms) {
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            queue.Enqueue(start);
            RoomBounds roomBounds = new RoomBounds(start);
            Dictionary<Vector2Int, List<TileBase>> positionTileDict = new Dictionary<Vector2Int, List<TileBase>>();
            RoomObject roomElementCollection = null;
            while (queue.Count > 0) {
                Vector2Int current = queue.Dequeue();
                int x = current.x - bounds.xMin;
                int y = current.y - bounds.yMin;
                if (x < 0 || x >= seen.GetLength(0) || y < 0 || y >= seen.GetLength(1)) {
                    continue;
                }
                Vector3Int vector = (Vector3Int) current;
                positionTileDict[current] = getTileList(vector, levelData.TileMapDict);
                if (current == levelData.SpawnPosition) {
                    levelData.SpawnRoomIndex = rooms.Count;
                }
                if (seen[x,y]) {
                    continue;
                }
                if (levelData.PositionRoomObjectCollection.ContainsKey(current)) {
                    roomElementCollection = levelData.PositionRoomObjectCollection[current];
                }
                seen[x,y] = true;
                if (levelData.GetTile(vector,TileMapLayer.Wall) == null && levelData.GetTile(vector,TileMapLayer.Door) == null) {
                    roomBounds.expand(current);
                    foreach (Vector2Int direction in directions)
                    {
                        Vector2Int neighbor = current + direction;
                        queue.Enqueue(neighbor);
                    }
                } 
            }
            if (roomElementCollection == null) {
                return;
            }
            bool enclosed = roomBounds.XMax != bounds.xMax && roomBounds.XMin != bounds.xMin && roomBounds.YMax != bounds.yMax && roomBounds.YMin != bounds.yMin;
            if (!enclosed) {
                //Debug.LogWarning($"Room not enclosed with start {start} and bounds {bounds}");
                return;
            }
            //roomBounds.expand(1); // Expand to include walls & doors
            HashSet<Vector2Int> seenBorder = new HashSet<Vector2Int>();
            List<RoomDoor> roomDoors = new List<RoomDoor>();
            // Depth first search the border of the room to find any gaps and doors
            // Has to be a wall on the border which is adjacent to a door
            foreach (var kvp in positionTileDict) {
                bool isWall = kvp.Value[1] != null;
                if (!isWall) {
                    continue;
                }
                /*
                bool hasAdjcaentDoor = false;
                foreach (Vector2Int directionVector in directions) {
                    Vector2Int pos = kvp.Key + directionVector;
                    if (levelData.GetTile((Vector3Int)pos,TileMapLayer.Door) != null) {
                        hasAdjcaentDoor = true;
                        break;
                    }
                }
                if (!hasAdjcaentDoor) {
                    continue;
                }
                */
                DFSBorder(kvp.Key,roomBounds,null,seenBorder, positionTileDict, roomDoors, levelData);
                Debug.Log(seenBorder.Count);
                break;
                // Found a tile on the border. Since the border is connected we can break after
                //break;
                
            }
            int DOOR_TILE_INDEX = 2;
            foreach (RoomDoor roomDoor in roomDoors) {
                if (roomDoor.GetLineDirection() == LineDirection.Horizontal) {
                    int y = roomDoor.StartPosition.y;
                    for (int x = roomDoor.StartPosition.x; x <= roomDoor.EndPosition.x; x++) {
                        Vector2Int vector = new Vector2Int(x,y);
                        if (positionTileDict.ContainsKey(vector)) {
                            positionTileDict[vector][DOOR_TILE_INDEX] = null;
                        }
                    }
                }
            }
            Dictionary<TileMapLayer, TileBase[,]> layerTileDict = new Dictionary<TileMapLayer, TileBase[,]>();
            List<TileMapLayer> layers = Enum.GetValues(typeof(TileMapLayer)).Cast<TileMapLayer>().ToList();
            Vector2Int size = roomBounds.Size;
            Debug.Log(size);
            Debug.Log(roomBounds);
            foreach (TileMapLayer layer in layers) {
                layerTileDict[layer] = new TileBase[size.x,size.y];
            }
            foreach (var kvp in positionTileDict) {
                int xIndex = kvp.Key.x - roomBounds.XMin;
                int yIndex = kvp.Key.y - roomBounds.YMin;      
                for (int i = 0; i < layers.Count; i++) {
                    TileMapLayer layer = layers[i];
                    TileBase tile = kvp.Value[i];
                    if (xIndex < 0 || yIndex < 0 || xIndex >= size.x || yIndex >= size.y) {
                        Debug.LogError($"Index out of bounds {xIndex}, {yIndex}");
                        continue;
                    }
                    layerTileDict[layer][xIndex,yIndex] = tile;
                }
            }
            rooms.Add(new Room(roomBounds,layerTileDict,roomDoors,roomElementCollection));
        }

        private static void DFSBorder(
            Vector2Int current, 
            RoomBounds roomBounds, 
            Direction? originDirection, 
            HashSet<Vector2Int> seenBorder, 
            Dictionary<Vector2Int, List<TileBase>> positionTileDict,
            List<RoomDoor> roomDoors,
            LevelData levelData
            ) {
            if (seenBorder.Contains(current)) {
                return;
            }
            seenBorder.Add(current);
            Vector3Int vector = (Vector3Int) current;
            bool wallTile = levelData.GetTile(vector,TileMapLayer.Wall) != null;
            bool doorTile = levelData.GetTile(vector,TileMapLayer.Door) != null;
            if (!wallTile && !doorTile) {
                return;
            }
            roomBounds.expand(current);
            if (!positionTileDict.ContainsKey(current)) {
                positionTileDict[current] = getTileList(vector,levelData.TileMapDict);
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
            
            // Don't search in a direction we've came from
            if (originDirection != Direction.Right) {
                DFSBorder(current+Vector2Int.left,roomBounds,Direction.Left,seenBorder, positionTileDict, roomDoors, levelData);
            }
            if (originDirection != Direction.Left) {
                DFSBorder(current+Vector2Int.right,roomBounds,Direction.Right,seenBorder, positionTileDict, roomDoors, levelData);
            }
            if (originDirection != Direction.Down) {
                DFSBorder(current+Vector2Int.up,roomBounds,Direction.Up,seenBorder, positionTileDict, roomDoors, levelData);
            }
            if (originDirection != Direction.Up) {
                DFSBorder(current+Vector2Int.down,roomBounds,Direction.Down,seenBorder, positionTileDict, roomDoors, levelData);
            }

        }

        private static List<TileBase> getTileList(Vector3Int vector, Dictionary<TileMapLayer, Tilemap> tileMapDict) {
            return new List<TileBase>{
                tileMapDict[TileMapLayer.Floor].GetTile(vector),
                tileMapDict[TileMapLayer.Wall].GetTile(vector),
                tileMapDict[TileMapLayer.Door].GetTile(vector)
            };
        }
    }
}


