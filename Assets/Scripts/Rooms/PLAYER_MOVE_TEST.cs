using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rooms;

namespace PlayerModule {
    public class PLAYER_MOVE_TEST : MonoBehaviour
    {
        Rigidbody2D rb;
        SpriteRenderer spriteRenderer;
        public PlayerWalkSFX playerWalkSFX;
        
        private bool isMoving = false;
        private bool isRunning = false;

        public float walkSpeed = 10f;
        public float runSpeed = 20f;

        public void Start() {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Update() {
            if (!Player.Instance.CanMove) {
                rb.velocity = Vector2.zero;
                return;
            }
            Vector3 moveDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                moveDirection += Vector3.up;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                moveDirection += Vector3.down;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                moveDirection += Vector3.left;
                spriteRenderer.flipX = true;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                moveDirection += Vector3.right;
                spriteRenderer.flipX= false;
            }

            isRunning = Input.GetKey(KeyCode.R);
            
            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            
            rb.velocity = moveDirection.normalized * currentSpeed;

            isMoving = moveDirection != Vector3.zero;
            if (isMoving) {
                if (isRunning)
                {
                    playerWalkSFX.playSound(PlayerWalkSFX.PlayerMovementSound.Run);
                }
                else
                {
                    playerWalkSFX.playSound(PlayerWalkSFX.PlayerMovementSound.Walk);
                }
            }
            
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            RoomDoorObject roomDoorObject = collision.gameObject.GetComponent<RoomDoorObject>();
            
            if (roomDoorObject != null) {
                roomDoorObject.switchRoom(transform);
            }
        }
    }

}


