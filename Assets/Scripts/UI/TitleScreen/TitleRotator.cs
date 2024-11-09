using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TitleScreen {
    
    public class TitleRotator : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private RotationCounter xCounter;
        [SerializeField] private RotationCounter yCounter;
        [SerializeField] private RotationCounter zCounter;
        private List<RotationCounter> counters;
        private bool speedUp;
        private int speedUpCounter = 0;
        public int SpeedUpModifier = 50;
        public void Start() {
            counters = new List<RotationCounter>();
            counters.Add(xCounter);
            counters.Add(yCounter);
            counters.Add(zCounter);
        }
        public void FixedUpdate() {
            if (speedUp) {
                speedUpCounter++;
                if (speedUpCounter > 10) {
                    speedUp = false;
                    speedUpCounter = 0;
                }
            }
            int iteration = speedUp ? SpeedUpModifier : 1;
            foreach (RotationCounter counter in counters) {
                counter.iterate(iteration);
            }
    
            float xRotation = counters[0].getCurrentValue();
            float yRotation = counters[1].getCurrentValue();
            float zRotation = counters[2].getCurrentValue();
            
            transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            speedUp = true;
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
            public void iterate(int amount) {
                if (reverse) {
                    count -= amount;
                } else {
                    count += amount;
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

