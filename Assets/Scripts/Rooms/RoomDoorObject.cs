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
                    light2D.transform.localPosition = Vector3.down/2f;
                    light2D.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                    break;
                case Direction.Up:
                    light2D.transform.localPosition = Vector3.up/2f;
                    break;
                case Direction.Right:
                    light2D.transform.localPosition = Vector3.left/2f;
                    light2D.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                    break;
                case Direction.Left:
                    light2D.transform.localPosition = Vector3.right/2f;
                    light2D.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
                    break;
            }
        }

        public void setClear(bool roomClear) {
            bool connectionClear = roomDoor.Connection.Room.isClear();
            if (connectionClear) {
                light2D.color = Color.green;
            } else if (!connectionClear && !roomClear) {
                light2D.color = Color.black;
            } else {
                light2D.color = Color.red;
            }
        }
        public void switchRoom() {
            if (!roomDoor.Room.isClear() && !roomDoor.Connection.Room.isClear()) {
                return;
            }
            
            if (roomDoor.Room.isClear()) {
                PlayerUI playerUI = Player.Instance.PlayerUI;
                PlayerLevelComponent playerLevelComponent = Player.Instance.GetComponent<PlayerLevelComponent>();
                int upgrades = playerLevelComponent.RemainingUpgrades;
                PlayerExperienceUI playerExperienceUI = playerUI.PlayerExperienceUI;
                if (upgrades > 0 && !playerExperienceUI.SelectorDisplayed) {
                    playerExperienceUI.displayLevelSelector(doSwitch);
                    return;
                } 
            }
            doSwitch();
        }

        private void doSwitch() {
            Transform playerTransform = Player.Instance.transform;
            Room room = roomDoor.Room;
            LevelManager.getInstance().CurrentLevel.loadRoom(roomDoor.Connection.Room);
            Vector3 spawnPosition = roomDoor.Connection.getEnterPosition(playerTransform.position);
            Player.Instance.SetPosition(spawnPosition);
        }
    }
}

