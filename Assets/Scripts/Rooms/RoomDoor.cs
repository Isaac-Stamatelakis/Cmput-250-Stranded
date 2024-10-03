using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rooms {
    public class RoomDoor {
        /// <summary>
        /// Represents a line through world space which connects rooms.
        /// </summary>
        private Vector3Int startPosition;
        private Vector3Int endPosition;
        public Vector3Int StartPosition => startPosition;
        public Vector3Int EndPosition => endPosition;
        public int Size => (int) (startPosition-endPosition).magnitude+1;
        private Room room;
        public Room Room => room;
        private RoomDoor connection;
        public RoomDoor Connection => connection;
        private Direction direction;
        public Direction Direction => direction;
        private BoundsInt bounds;
        public RoomDoor(Vector3Int point) {
            this.startPosition = point;
            this.endPosition = point;
        }
        public void setRoom(Room room) {
            this.room = room;
        }
        /// <summary>
        ///
        /// </summary>
        public bool expand(Vector3Int adjacentPosition) {
            if (!pointOnLine(adjacentPosition)) {
                return false;
            }
            // X Axis
            if (adjacentPosition.x > endPosition.x) {
                endPosition.x = adjacentPosition.x;
            } else if (adjacentPosition.x < startPosition.x) {
                startPosition.x = adjacentPosition.x;
            } // Y Axis
            if (adjacentPosition.y > endPosition.y) {
                endPosition.y = adjacentPosition.y;
            } else if (adjacentPosition.y < startPosition.y) {
                startPosition.y = adjacentPosition.y;
            }
            return true;
        }

        public void setConnection(RoomDoor connection, Direction direction) {
            this.connection = connection;
            this.direction = direction;
        }

        public bool isParallelTo(RoomDoor roomDoor) {
            if (roomDoor.GetLineDirection() != GetLineDirection()) {
                return false;
            }
            LineDirection lineDirection = GetLineDirection();
            if (lineDirection == LineDirection.Horizontal) {
                return startPosition.x == roomDoor.startPosition.x && 
                    endPosition.x == roomDoor.endPosition.x &&
                    startPosition.y != roomDoor.startPosition.y;
            } else {
                return startPosition.y == roomDoor.startPosition.y && 
                    endPosition.y == roomDoor.endPosition.y &&
                    startPosition.x != roomDoor.startPosition.x;
            }
            

                    

      
        }

        public Vector3 getMidpoint() {
            return new Vector3((startPosition.x+endPosition.x)/2f,(startPosition.y+endPosition.y)/2f)*GlobalUtils.TILE_SIZE;
        }

        public int distanceFrom(RoomDoor roomDoor) {
            if (!isParallelTo(roomDoor)) {
                return int.MaxValue;
            }
            LineDirection direction = GetLineDirection();
            if (direction == LineDirection.Vertical) {
                return startPosition.x - roomDoor.startPosition.x;
            } else {
                return startPosition.y - roomDoor.startPosition.y;
            }
        }

        public LineDirection GetLineDirection() {
            if (startPosition.x == endPosition.x) {
                return LineDirection.Vertical;
            }
            if (startPosition.y == EndPosition.y) {
                return LineDirection.Horizontal;
            }
            return LineDirection.Horizontal;
        }

        public bool pointOnLine(Vector3Int point) {
            return 
                (point.x == startPosition.x && point.x == endPosition.x) ||
                (point.y == startPosition.y && point.y == endPosition.y);
        }

        public bool adjacentPointOnLine(Vector3Int point) {
            if (!pointOnLine(point)) {
                return false;
            }
            if (pointAdjacent(startPosition,point)) {
                return true;
            }
            return pointAdjacent(endPosition,point);
        }

        private bool pointAdjacent(Vector3Int a, Vector3Int b) {
            Vector3Int dif = a-b;
            return dif.x == 1 || dif.x == -1 || dif.y == -1 || dif.y == 1;
        }

        public Vector3 getEnterPosition(Vector3 playerPosition) {
            Vector3 midpoint = getMidpoint();
            float seperation = GlobalUtils.TILE_SIZE;
            switch (direction) {
                case Direction.Right:
                    return new Vector3(midpoint.x-seperation,playerPosition.y,playerPosition.z);
                case Direction.Left:
                    return new Vector3(midpoint.x+seperation+GlobalUtils.TILE_SIZE,playerPosition.y,playerPosition.z);
                case Direction.Up:
                    return new Vector3(playerPosition.x,midpoint.y+seperation+GlobalUtils.TILE_SIZE,playerPosition.z);
                case Direction.Down:
                    return new Vector3(playerPosition.x,midpoint.y-seperation,playerPosition.z);
                default:
                    throw new System.Exception($"Did not cover case for {direction}");
            }
            
        }

        public override string ToString()
        {
            return $"Direction: {GetLineDirection()} Start: {startPosition}, End: {endPosition}";
        }
    }
}