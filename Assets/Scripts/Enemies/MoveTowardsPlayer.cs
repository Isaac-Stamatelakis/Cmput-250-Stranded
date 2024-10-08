using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;

public class MoveTowardsPlayer : MonoBehaviour
{

    public float moveSpeed = 1.0f;
    private bool hasCollided = false;
    private Player player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public float ranMin = 1.0f;
    public float ranMax = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasCollided) {
            Vector3 randomized = new Vector3(Random.Range(ranMin,ranMax),Random.Range(ranMin,ranMax),0);
            Vector2 moveToTarget = ((player.transform.position + randomized) - transform.position).normalized;
            rb.velocity = (Vector2)(moveToTarget * moveSpeed);
        } else {
            rb.velocity = new Vector2(0.0f, 0.0f);
        }
        spriteRenderer.flipX = transform.position.x > player.transform.position.x;
    }

    private void OnCollisionEnter2D(Collision2D coll) {
        Rigidbody2D rb = coll.gameObject.GetComponent<Rigidbody2D>();
        
        if (coll.gameObject.tag == "Player") {
            this.rb.bodyType = RigidbodyType2D.Kinematic;
            hasCollided = true;
        }
    }

    private void OnCollisionExit2D(Collision2D coll) {
        this.rb.bodyType = RigidbodyType2D.Dynamic;
        hasCollided = false;
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Player") {
            this.rb.bodyType = RigidbodyType2D.Kinematic;
            hasCollided = true;
        } 
    }

    private void OnTriggerExit2D(Collider2D coll) {
        rb.bodyType = RigidbodyType2D.Dynamic;
        hasCollided = false;
    }
}
