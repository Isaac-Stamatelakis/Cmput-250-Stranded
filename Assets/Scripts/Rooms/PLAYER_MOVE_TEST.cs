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
        public Animator animator;

        public bool isMoving = false;   
        public bool isRunning = false;
        private PlayerAttack playerAttack;

        public float walkSpeed = 10f;
        public float runSpeed = 20f;

        Vector2 moveDirection;

        public void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public void Update() {
            if (!Player.Instance.CanMove) {
                rb.velocity = Vector2.zero;
                return;
            }
            moveDirection = Vector2.zero;


            bool moveUp = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            bool moveDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
            bool moveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
            bool moveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

            // Handle vertical movement
            if (moveUp && !moveDown)
            {
                moveDirection += Vector2.up;
                setAnimationsFalse();
                animator.SetBool("isBack", true);
            }
            else if (moveDown && !moveUp)
            {
                moveDirection += Vector2.down;
                setAnimationsFalse();
                animator.SetBool("isForwards", true);
            }

            // Handle horizontal movement
            if (moveLeft && !moveRight)
            {
                moveDirection += Vector2.left;
                setAnimationsFalse();
                animator.SetBool("isLeft", true);
            }
            else if (moveRight && !moveLeft)
            {
                moveDirection += Vector2.right;
                setAnimationsFalse();
                animator.SetBool("isRight", true);
            }
            
            isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            if (moveDirection != Vector2.zero)
            {
                moveDirection = moveDirection.normalized * currentSpeed;
            }
            else
            {
                moveDirection = Vector2.zero;
                setAnimationsFalse();
            }
            

            isMoving = moveDirection != Vector2.zero;
            if (isMoving)
            {
                if (isRunning)
                {
                    animator.speed = 1.5f;
                    playerWalkSFX.playSound(PlayerWalkSFX.PlayerMovementSound.Run);
                }
                else
                {
                    animator.speed = 1.0f;
                    playerWalkSFX.playSound(PlayerWalkSFX.PlayerMovementSound.Walk);
                }
            }
        }

        void FixedUpdate()
        {
            if (moveDirection != Vector2.zero)
            {
                rb.MovePosition(rb.position + moveDirection * Time.fixedDeltaTime);
            } else {
                rb.velocity = Vector2.zero;
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

        void setAnimationsFalse()
        {
            animator.SetBool("isForwards", false);
            animator.SetBool("isRight", false);
            animator.SetBool("isLeft", false);
            animator.SetBool("isBack", false);
        }
    }
}
