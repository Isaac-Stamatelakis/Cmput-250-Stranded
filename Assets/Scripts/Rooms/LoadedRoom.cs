using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rooms {
    public class LoadedRoom : MonoBehaviour
    {
        public Transform enemeyContainer;
        public Transform triggerContainer;
        public Transform interactableContainer;
        public Transform doorContainer;
        private Room room;
        public void reset() {
            GlobalUtils.deleteChildren(enemeyContainer);
            GlobalUtils.deleteChildren(triggerContainer);
            GlobalUtils.deleteChildren(interactableContainer);
            GlobalUtils.deleteChildren(doorContainer);
            if (room != null) {
                room.unload();
            }
            
        }
        public void setRoom(Room room) {
            this.room = room;
        }
    }
}

