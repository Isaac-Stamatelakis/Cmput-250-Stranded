using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;
using Dialogue;
using System.Linq;
using Rooms;

public class DatePlayer : MonoBehaviour
{
    public float followSpeed = 8f;      
    public Material HighlightShader;
    private Material defaultShader;
    private bool isCollidingWithPlayer = false;  
    public List<DialogObject> RandomDialogues;
    private List<DialogObject> selectableRandomDialogues;
    private Queue<DialogObject> unselectableRandomDialogues;
    public float boostSpeed = 20f;
    public float followDistance = 2f;     
    public float stopDistance = 1f;      
    public float maxDistance = 10f;     

    private Vector2 playerOffset = new Vector2(0, -0.5f); 
    private bool isCollidingWithPlayer = false;

    private Vector2 lastPosition;         
    private int stuckFrames = 0;          
    private int maxStuckFrames = 60;     

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRandomDialogues(RandomDialogues);
        this.defaultShader = spriteRenderer.material;
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
    public void Talk() {
        if (DialogUIController.Instance.ShowingDialog) {
            return;
        }
        DialogObject dialogObject = cycleDialogues();
        DialogUIController.Instance.DisplayDialogue(dialogObject);
    }

    private DialogObject cycleDialogues() {
        if (RandomDialogues.Count == 0) {
            return null;
        }
        if (RandomDialogues.Count == 1) {
            return RandomDialogues[0];
        }
        int ran = UnityEngine.Random.Range(0,selectableRandomDialogues.Count);
        DialogObject randomlySelected = selectableRandomDialogues[ran];
        selectableRandomDialogues.RemoveAt(ran);
        unselectableRandomDialogues.Enqueue(randomlySelected);

        DialogObject newlySelectable = unselectableRandomDialogues.Dequeue();
        selectableRandomDialogues.Add(newlySelectable);
        return randomlySelected;
    }

    public void setHighlight(bool highlight) {
        if (spriteRenderer==null) {
            return;
        }
        spriteRenderer.material = highlight ? HighlightShader : defaultShader;
    }

    private void ShuffleList(List<DialogObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void SetRandomDialogues(List<DialogObject> dialogObjects) {
        this.RandomDialogues = dialogObjects;
        if (dialogObjects == null || dialogObjects.Count == 1) {
            return;
        }
        List<DialogObject> shuffledList = new List<DialogObject>(dialogObjects);
        ShuffleList(shuffledList);

        selectableRandomDialogues = new List<DialogObject>();
        unselectableRandomDialogues = new Queue<DialogObject>();
        for (int i = 0; i < shuffledList.Count; i++) {
            if (i % 2 == 0) {
                selectableRandomDialogues.Add(shuffledList[i]);
            } else {
                unselectableRandomDialogues.Enqueue(shuffledList[i]);
            }
        }

    }
}
