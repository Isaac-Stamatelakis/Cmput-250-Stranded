using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Rooms;
public class PLAYER_MOVE_TEST : MonoBehaviour
{
    Rigidbody2D rb;
    
    // Variables to track movement and running state
    private bool isMoving = false;
    private bool isRunning = false;

    // Speed variables for walking and running
    public float walkSpeed = 10f;
    public float runSpeed = 20f;

    public void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update() {
        Vector3 moveDirection = Vector3.zero;

        // Check for key presses to move the player (WASD and Arrow keys)
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
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection += Vector3.right;
        }

        // Check if the player is running by holding the "R" key
        isRunning = Input.GetKey(KeyCode.R);
        
        // Adjust player speed based on whether the player is running or walking
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        
        // Apply movement to the player
        rb.velocity = moveDirection.normalized * currentSpeed;

        // Handle walking or running sound effects based on movement
        if (moveDirection != Vector3.zero)
        {
            // Player is moving
            if (!isMoving)
            {
                if (isRunning)
                {
                    // Play running sound when running
                    AudioController.Instance.PlayPlayerRunning();
                }
                else
                {
                    // Play walking sound when walking
                    AudioController.Instance.PlayPlayerWalking();
                }
                isMoving = true;
            }
        }
        else
        {
            // Player stopped moving
            if (isMoving)
            {
                AudioController.Instance.StopPlayerWalking();
                isMoving = false;
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

