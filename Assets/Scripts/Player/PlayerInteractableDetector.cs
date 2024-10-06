using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlayerModule {
    public class PlayerInteractableDetector : MonoBehaviour
    {
        private LayerMask interactableLayer;
        private IInteractableGameObject currentObject;
        public void Start() {
            interactableLayer = LayerMask.GetMask("Interactables");
        }
        public void FixedUpdate() {
            DetectInteractables();
            if (currentObject != null) {
                InteractableUIController.Instance.display(InteractableDisplayType.Interactable,currentObject.getInteractText());
                return;
            }
            bool nearDate = IsNearDate();
            if (nearDate) {
                InteractableUIController.Instance.display(InteractableDisplayType.TalkToDate,"To Talk to Your Date");
                return;
            }
            InteractableUIController.Instance.hide();
        }

        public void Update() {
            if (currentObject == null) {
                return;
            }
            if (Input.GetKeyDown(KeyCode.E)) {
                currentObject.interact();
            }
        }

        private void DetectInteractables() {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(
                origin: transform.position,
                GlobalUtils.PLAYER_INTERACTABLE_DETECTION_RANGE,
                direction: Vector2.zero,
                distance: Mathf.Infinity,
                layerMask: interactableLayer
            );
            IInteractableGameObject closestHit = GetClosestHit(hits);
            if (closestHit == null) {
                if (currentObject != null) {
                    currentObject.unhighlight();
                    currentObject = null;
                }
                return;
            }
            
            if (closestHit.Equals(currentObject)) {
                return;
            }
            if (currentObject != null) {
                currentObject.unhighlight();
            }
            closestHit.highlight();
            currentObject = closestHit;
        }

        private bool IsNearDate() {
            return false;
        }

        private IInteractableGameObject GetClosestHit(RaycastHit2D[] hits) {
            if (hits.Length == 0) {
                return null;
            }
            float closestDistance = float.PositiveInfinity;
            IInteractableGameObject closestHit = null;
            for (int i = 0; i < hits.Length; i++) {
                // Because we are get an interface, we have to get all components which implement the interface from the collided object
                IInteractableGameObject[] hitInteractableGameObjects = hits[i].collider.gameObject.GetComponentsInChildren<IInteractableGameObject>();
                if (hitInteractableGameObjects.Length == 0) {
                    continue;
                }
                // Simply take the first component which implements the interface
                IInteractableGameObject hitInteractableGameObject = hitInteractableGameObjects[0];
                // Warn if an object has multiple components as it causes indeterminant behavior
                if (hitInteractableGameObjects.Length > 1) {
                    Debug.LogWarning($"Interactable Game Object {hits[i].collider.name} has multiple interactable components");
                }
                float distance = Vector3.Distance(transform.position,hits[i].transform.position);
                bool closest = distance < closestDistance;
                if (closest) {
                    closestHit = hitInteractableGameObject;
                    closestDistance = distance;
                }
            }
            return closestHit;
        }
    }
}

