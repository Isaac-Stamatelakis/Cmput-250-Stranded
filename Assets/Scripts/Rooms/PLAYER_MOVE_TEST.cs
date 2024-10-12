using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rooms;

namespace PlayerModule
{
    public class PLAYER_MOVE_TEST : MonoBehaviour
    {
        Rigidbody2D rb;
        SpriteRenderer spriteRenderer;
        public PlayerWalkSFX playerWalkSFX;

        public bool isMoving = false;   
        public bool isRunning = false;  

        public float walkSpeed = 10f;
        public float runSpeed = 20f;

        Vector2 moveDirection;

        public void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public void Update() {
            moveDirection = Vector2.zero;
            if (!Player.Instance.CanMove) {
                return;
            }
            

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                moveDirection += Vector2.up;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                moveDirection += Vector2.down;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                moveDirection += Vector2.left;
                spriteRenderer.flipX = true;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                moveDirection += Vector2.right;
                spriteRenderer.flipX = false;
            }

            isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            moveDirection = moveDirection.normalized * currentSpeed;

            isMoving = moveDirection != Vector2.zero;
            if (isMoving)
            {
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

        void FixedUpdate()
        {
            if (moveDirection != Vector2.zero)
            {
                rb.MovePosition(rb.position + moveDirection * Time.fixedDeltaTime);
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            RoomDoorObject roomDoorObject = collision.gameObject.GetComponent<RoomDoorObject>();

            if (roomDoorObject != null)
            {
                roomDoorObject.switchRoom(transform);
            }
        }
    }
}
