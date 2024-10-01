using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rooms {
    public abstract class TriggerableEvent : MonoBehaviour
    {
        public bool repeatable = false;
        private bool activated = false;
        public void OnTriggerEnter2D(Collider2D collider2D) {
            if (activated && !repeatable) {
                return;
            }
            activated = true;
            trigger();
        }

        public abstract void trigger();

    }
}

