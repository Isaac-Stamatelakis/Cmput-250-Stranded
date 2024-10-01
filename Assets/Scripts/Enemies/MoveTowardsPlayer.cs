using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{

    public float moveSpeed = 1.0f;
    private bool hasCollided = false;
    public GameObject player;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = GameObject.Find("SamplePlayer").transform.position;

        if (!hasCollided) {
            Vector2 moveToTarget = (player.transform.position - transform.position).normalized;
            rb.velocity = (Vector2)(moveToTarget * moveSpeed);
        } else {
            rb.velocity = new Vector2(0.0f, 0.0f);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D coll) {
        Rigidbody2D rb = coll.gameObject.GetComponent<Rigidbody2D>();
        if (coll.gameObject.name == ("SamplePlayer")) {
            Debug.Log("has collided");
            this.rb.bodyType = RigidbodyType2D.Kinematic;
            hasCollided = true;
        }
    }

    private void OnCollisionExit2D(Collision2D coll) {
        this.rb.bodyType = RigidbodyType2D.Dynamic;
        hasCollided = false;
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        this.rb.bodyType = RigidbodyType2D.Kinematic;
        hasCollided = true;
    }

    private void OnTriggerExit2D(Collider2D coll) {
        rb.bodyType = RigidbodyType2D.Dynamic;
        hasCollided = false;
    }
}
