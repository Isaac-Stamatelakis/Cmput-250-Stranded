using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rooms;
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
            
        }
        public void Update() {
            DatePlayer datePlayer = Player.Instance.DatePlayer;
            if (currentObject != null && currentObject.isInteractable()) {
                InteractableUIController.Instance.display(InteractableDisplayType.Interactable,currentObject.getInteractText(),currentObject.getPosition());
                datePlayer.setHighlight(false);
                if (Input.GetKeyDown(KeyCode.E)) {
                    currentObject.interact();
                }
                return;
            }
            bool nearDate = IsNearDate();
            datePlayer.setHighlight(nearDate);
            if (nearDate) {
                InteractableUIController.Instance.display(InteractableDisplayType.TalkToDate,"Talk",datePlayer.transform.position);
                if (Input.GetKeyDown(KeyCode.E)) {
                    Player.Instance.DatePlayer.Talk();
                }
                return;
            }
            InteractableUIController.Instance.hide();
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
                if (currentObject != null && currentObject.isInteractable()) {
                    currentObject.unhighlight();
                }
                currentObject = null;
                return;
            }
            
            if (closestHit.Equals(currentObject)) {
                return;
            }
            if (currentObject != null && currentObject.isInteractable()) {
                currentObject.unhighlight();
            }
            closestHit.highlight();
            currentObject = closestHit;
        }

        private bool IsNearDate() {
            DatePlayer datePlayer = Player.Instance.DatePlayer;
            float distance = Vector2.Distance(transform.position,datePlayer.transform.position); // Use Vector2 to ignore z value
            return distance < GlobalUtils.PLAYER_INTERACTABLE_DETECTION_RANGE;
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
                if (!hitInteractableGameObject.isInteractable()) {
                    continue;
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

