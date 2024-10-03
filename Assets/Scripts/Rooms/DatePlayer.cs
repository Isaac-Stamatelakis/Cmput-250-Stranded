using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rooms;
public class DatePlayer : MonoBehaviour
{
    public Transform playerTransform; 
    public float followSpeed = 8f;    
    public float followDistance = 5f;  
    public float stopDistance = 2f;    

    private Rigidbody2D rb;
    private bool isCollidingWithPlayer = false;  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        
        if (!isCollidingWithPlayer)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer > stopDistance)
            {
                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                rb.velocity = directionToPlayer * followSpeed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    // Stop the love interest when colliding with the player
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
        }

        RoomDoorObject roomDoorObject = collision.gameObject.GetComponent<RoomDoorObject>();
        if (roomDoorObject != null)
        {
            roomDoorObject.switchRoom(transform);
        }
    }

    // Allow the love interest to move again when they stop colliding with the player
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
        }
    }
}