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
        PlayerLevelComponent playerLevelComponent;
        public PlayerWalkSFX playerWalkSFX;
        public Animator animator;

        public bool isMoving = false;   
        //public bool isRunning = false;
        private PlayerAttack playerAttack;
        public bool isInCutscene = false;

        public float walkSpeed = 20f;
        //public float runSpeed = 20f;

        Vector2 moveDirection;

        public void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerLevelComponent = GetComponent<PlayerLevelComponent>();

        }
        public void Update() {
            moveDirection = Vector2.zero;
            if (!Player.Instance.CanMove) {
                setAnimationsFalse();
                rb.velocity = Vector2.zero;
                return;
            }

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
            
            //isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float currentSpeed = walkSpeed;
            if (playerLevelComponent.HasUpgrade(PlayerUpgrade.Speed))
            {
                currentSpeed *= PlayerUpgradeUtils.SPEED_UPGRADE_MODIFIER;
            }
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
            //rb.velocity = moveDirection * walkSpeed;
            //if (isMoving)
            //{
                //if (isRunning)
                //{
                //    animator.speed = 1.5f;
                //    playerWalkSFX.playSound(PlayerWalkSFX.PlayerMovementSound.Run);
                //}
                //else
                //{
                //    animator.speed = 1.0f;
                //    playerWalkSFX.playSound(PlayerWalkSFX.PlayerMovementSound.Walk);
                //}
            //}
        }

        void FixedUpdate()
        {

            if (moveDirection != Vector2.zero)
            {
                rb.MovePosition(rb.position + moveDirection * Time.fixedDeltaTime);
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
