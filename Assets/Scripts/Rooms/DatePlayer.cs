using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;
using Rooms;

public class DatePlayer : MonoBehaviour
{
    public float boostSpeed = 20f;
    public float followDistance = 2f;     
    public float stopDistance = 1f;      
    public float maxDistance = 10f;     

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 playerOffset = new Vector2(0, -0.5f); 
    private bool isCollidingWithPlayer = false;

    private Vector2 lastPosition;         
    private int stuckFrames = 0;          
    private int maxStuckFrames = 60;     

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position; 
    }

    void FixedUpdate()  
    {
        if (!Player.Instance.CanMove) {
    return;
            }
        if (!isCollidingWithPlayer)
        {
            Transform playerTransform = Player.Instance.transform;
            PLAYER_MOVE_TEST playerMoveScript = Player.Instance.GetComponent<PLAYER_MOVE_TEST>();

            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            float currentFollowSpeed = playerMoveScript.isRunning ? playerMoveScript.runSpeed : playerMoveScript.walkSpeed;

            if (distanceToPlayer > maxDistance)
            {
                currentFollowSpeed = boostSpeed; 
            }

            if (distanceToPlayer > stopDistance)
            {
                Vector2 targetPosition = (Vector2)playerTransform.position + playerOffset;
                Vector2 directionToPlayer = (targetPosition - (Vector2)transform.position).normalized;

                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer);

                if (hit.collider != null && hit.collider.CompareTag("Wall"))
                {
                    Vector2 avoidanceDirection = Vector3.Cross(directionToPlayer, Vector3.forward).normalized;
                    rb.MovePosition(rb.position + (directionToPlayer + avoidanceDirection * 0.5f) * currentFollowSpeed * Time.fixedDeltaTime);
                }
                else 
                {
                    rb.MovePosition(rb.position + directionToPlayer * currentFollowSpeed * Time.fixedDeltaTime);
                    stuckFrames = 0; 
                }

                // Check if DatePlayer hasn't moved for too long
                if (Vector2.Distance(transform.position, lastPosition) < 0.05f) // Minimal movement detected
                {
                    stuckFrames++;
                    if (stuckFrames >= maxStuckFrames)
                    {
                        // Force random movement if stuck for too long
                        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                        rb.MovePosition(rb.position + randomDirection * boostSpeed * Time.fixedDeltaTime);
                        stuckFrames = 0; // Reset stuck frames after forcing movement
                    }
                }
                else
                {
                    stuckFrames = 0;
                }

                lastPosition = transform.position;
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
