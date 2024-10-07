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
    public float followDistance = 5f;  
    public float stopDistance = 2f;    
    public Material HighlightShader;
    private Material defaultShader;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isCollidingWithPlayer = false;  
    public List<DialogObject> RandomDialogues;
    private List<DialogObject> selectableRandomDialogues;
    private Queue<DialogObject> unselectableRandomDialogues;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRandomDialogues(RandomDialogues);
        this.defaultShader = spriteRenderer.material;
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
