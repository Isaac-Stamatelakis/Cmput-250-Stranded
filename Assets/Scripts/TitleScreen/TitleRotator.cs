using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TitleScreen {
    
    public class TitleRotator : MonoBehaviour
    {
        [SerializeField] private RotationCounter xCounter;
        [SerializeField] private RotationCounter yCounter;
        [SerializeField] private RotationCounter zCounter;
        private List<RotationCounter> counters;
        public void Start() {
            counters = new List<RotationCounter>();
            counters.Add(xCounter);
            counters.Add(yCounter);
            counters.Add(zCounter);
        }
        public void FixedUpdate() {
            foreach (RotationCounter counter in counters) {
                counter.iterate();
            }
            transform.rotation = Quaternion.Euler(counters[0].getCurrentValue(), counters[1].getCurrentValue(), counters[2].getCurrentValue());
        }

        [System.Serializable]
        private class RotationCounter {
            private int count = 0;
            private bool reverse;
            public int Max = 100;
            public float Modifier = 0.25f;
            public float getCurrentValue() {
                return count*Modifier;
            }
            public void iterate() {
                if (reverse) {
                    count --;
                } else {
                    count ++;
                }
                if (reverse && count < -Max) {
                    reverse = !reverse;
                } else if (!reverse && count > Max) {
                    reverse = !reverse;
                }
            }
        }
    }
}

