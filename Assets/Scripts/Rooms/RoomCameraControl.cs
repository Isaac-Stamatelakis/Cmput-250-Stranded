using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rooms {
    public class RoomCameraControl : MonoBehaviour
    {
        private RoomBounds bounds;
        public void setBounds(RoomBounds bounds) {
            this.bounds = bounds;
        }
        private float height;
        private float width;

        public void Start() {
            Vector3[] frustumCorners = new Vector3[4];
            Camera camera = GetComponent<Camera>();
            camera.CalculateFrustumCorners(
                new Rect(0, 0, 1, 1), // Use full screen for corners calculation
                camera.transform.position.y, // Distance to calculate corners from
                Camera.MonoOrStereoscopicEye.Mono, // Use Mono for non-stereoscopic cameras
                frustumCorners); // Output array for corners

            // Determine the area covered by the camera
            height = 2f * camera.orthographicSize;
            width = height * camera.aspect;
        }
        public void Update() {
            // TODO CHANGE THIS SO ITS ONLY CALLED WHEN THE PLAYER MOVES
            Transform playerTransform = transform.parent;
            Vector3 position = transform.localPosition;
            bool outLeft = playerTransform.position.x-width/2 < bounds.XMin;
            bool outRight = playerTransform.position.x + width/2 > bounds.XMax+1;
            if (width >= bounds.XMax+1-bounds.XMin) {
                position.x = (bounds.XMin + bounds.XMax) / 2 - playerTransform.position.x;
            } else if (outLeft) {
                position.x = bounds.XMin+width/2-playerTransform.position.x;
            } else if (outRight) {
                position.x = bounds.XMax-width/2+1-playerTransform.position.x;
            } else {
                position.x = 0;
            }
            bool outBottom = playerTransform.position.y - height/2 < bounds.YMin;
            bool outTop = playerTransform.position.y + height/2 > bounds.YMax+1;
            if (height >= bounds.YMax+1-bounds.YMin) {
                position.y = (bounds.YMax+bounds.YMin)/2f-playerTransform.position.y;
            } else if (outTop) {
                position.y = bounds.YMax-height/2+1 - playerTransform.position.y;
            } else if (outBottom) {
                position.y = bounds.YMin+height/2 - playerTransform.position.y;
            } else {
                position.y = 0;
            }

            transform.localPosition = position;
        }
    }
}


