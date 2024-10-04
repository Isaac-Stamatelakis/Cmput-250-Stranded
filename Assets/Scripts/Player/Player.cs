using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class Player : MonoBehaviour
    {
        private static Player instance;
        public static Player Instance => instance;
        public void Awake() {
            instance = this;
        }
    }

}
