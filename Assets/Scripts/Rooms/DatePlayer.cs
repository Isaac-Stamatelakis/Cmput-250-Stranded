using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;

using Rooms;
public class DatePlayer : MonoBehaviour
{
    public float followSpeed = 8f;    
    public float followDistance = 5f;  
    public float stopDistance = 2f;    

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isCollidingWithPlayer = false;  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
        if (!isCollidingWithPlayer)
        {
            Transform playerTransform = Player.Instance.transform;
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
        spriteRenderer.flipX = transform.position.x < Player.Instance.transform.position.x;

    }

    // Stop the love interest when colliding with the player
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
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
