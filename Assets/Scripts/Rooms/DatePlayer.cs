using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;
using Rooms;

public class DatePlayer : MonoBehaviour
{
    public float walkFollowSpeed = 8f;    
    public float runFollowSpeed = 16f;    
    public float boostSpeed = 20f;       
    public float followDistance = 5f;    
    public float stopDistance = 2f;      
    public float maxDistance = 10f;       

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
            PLAYER_MOVE_TEST playerMoveScript = Player.Instance.GetComponent<PLAYER_MOVE_TEST>();

            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer > stopDistance)
            {
                float currentFollowSpeed = playerMoveScript.isRunning ? runFollowSpeed : walkFollowSpeed;

                if (distanceToPlayer > maxDistance)
                {
                    currentFollowSpeed = boostSpeed;
                }

                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                rb.velocity = directionToPlayer * currentFollowSpeed;
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
        }
    }
}
