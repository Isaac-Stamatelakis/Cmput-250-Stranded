using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Rooms;
public class PLAYER_MOVE_TEST : MonoBehaviour
{
    Rigidbody2D rb;
    public void Start() {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Update() {
        Vector3 moveDirection = Vector3.zero;

        // Check for key presses
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector3.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += Vector3.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector3.right;
        }
        rb.velocity = moveDirection.normalized * 15;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log("player collide");
        RoomDoorObject roomDoorObject = collision.gameObject.GetComponent<RoomDoorObject>();
           
        if (roomDoorObject != null) {
            roomDoorObject.switchRoom(transform);
        }
    }
}
