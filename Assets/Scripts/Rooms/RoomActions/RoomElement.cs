using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rooms {
    public abstract class RoomElement : MonoBehaviour
    {
        
    }

    public class RoomElementCollection {
        public string RoomName;
        public RoomElement[] roomElements;

        public RoomElementCollection(string roomName, RoomElement[] roomElements)
        {
            RoomName = roomName;
            this.roomElements = roomElements;
        }
    }
}

