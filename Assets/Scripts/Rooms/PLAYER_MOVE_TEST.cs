using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Rooms;
public class PLAYER_MOVE_TEST : MonoBehaviour
{
    Rigidbody2D rb;
    
    private bool isMoving = false;
    private bool isRunning = false;

    public float walkSpeed = 10f;
    public float runSpeed = 20f;

    public void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update() {
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
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection += Vector3.right;
        }

        isRunning = Input.GetKey(KeyCode.R);
        
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        
        rb.velocity = moveDirection.normalized * currentSpeed;

        if (moveDirection != Vector3.zero)
        {
            if (!isMoving)
            {
                if (isRunning)
                {
                    AudioController.Instance.PlayPlayerRunning();
                }
                else
                {
                    AudioController.Instance.PlayPlayerWalking();
                }
                isMoving = true;
            }
        }
        else
        {
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

