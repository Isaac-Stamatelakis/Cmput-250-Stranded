using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;
using UnityEngine.Rendering.Universal;

namespace Rooms {
    public class RoomDoorObject : MonoBehaviour
    {
        [SerializeField] private Light2D light2D;
        [SerializeField] private BoxCollider2D boxCollider2D;
        private RoomDoor roomDoor;
        public void setRoomDoor(RoomDoor roomDoor) {
            this.roomDoor = roomDoor;
            LineDirection lineDirection = roomDoor.GetLineDirection();
            Vector2 size = new Vector3(1.25f,roomDoor.Size) * GlobalUtils.TILE_SIZE;
            if (lineDirection == LineDirection.Horizontal) {
                boxCollider2D.size = new Vector2(size.y,size.x);
            } else {
                boxCollider2D.size = size;
            }
            transform.position = roomDoor.getMidpoint()+new Vector3(0.5f,0.5f);
            setClear(roomDoor.Room.isClear());
            Direction direction = roomDoor.Direction;
            switch (direction) {
                case Direction.Down:
                    light2D.transform.localPosition = Vector3.up;
                    break;
                case Direction.Up:
                    light2D.transform.localPosition = Vector3.down;
                    break;
                case Direction.Right:
                    light2D.transform.localPosition = Vector3.right;
                    break;
                case Direction.Left:
                    light2D.transform.localPosition = Vector3.left;
                    break;
            }
        }

        public void setClear(bool roomClear) {
            bool connectionClear = roomDoor.Connection.Room.isClear();
            if (connectionClear && roomClear) {
                light2D.color = Color.green;
            } else if (connectionClear || roomClear) {
                light2D.color = Color.yellow;
            } else {
                light2D.color = Color.black;
            }
        }
        public void switchRoom(Transform playerTransform) {
            if (!roomDoor.Room.isClear() && !roomDoor.Connection.Room.isClear()) {
                return;
            }
            Room room = roomDoor.Room;
            LevelManager.getInstance().CurrentLevel.loadRoom(roomDoor.Connection.Room);
            Vector3 spawnPosition = roomDoor.Connection.getEnterPosition(playerTransform.position);
            Player.Instance.SetPosition(spawnPosition);
        }
    }
}

