using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;

public class NavmeshFollow : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent zom;
    private UnityEngine.AI.NavMeshObstacle obs;

    private Animator anim;
    private Player player;
    private Vector3 target;
    private bool hasCollided;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    public int speed = 5;
    public float animSpeed = 0.1f;
    public Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        anim.speed = animSpeed;

        zom = GetComponent<UnityEngine.AI.NavMeshAgent>();
        zom.updateRotation = false;
        zom.updateUpAxis = false;

        obs = GetComponent<UnityEngine.AI.NavMeshObstacle>();
        obs.enabled = false;
        obs.carveOnlyStationary = false;
        obs.carving = true;

        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        player = Player.Instance;
        hasCollided = false;

        zom.speed = speed;

        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!hasCollided) {
            anim.SetBool("isWalking", true);

            obs.enabled = false;
            obs.carving = false;
            StartCoroutine(moveTo(player.transform.position));
        } else {
            anim.SetBool("isWalking", false);

            zom.enabled = false;
            obs.carving = true;
            StartCoroutine(stop());
        }

        sr.flipX = transform.position.x > player.transform.position.x;
    }

    private IEnumerator moveTo(Vector2 position) {
        yield return null;
        zom.enabled = true;
        zom.SetDestination(position);
    }

    private IEnumerator stop() {
        yield return null;
        obs.enabled = true;
        
    }

    private void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "Player") {
            anim.speed = animSpeed * .35f;
            anim.SetBool("isAttacking", true);
            Debug.Log("collided with player");
            hasCollided = true;

            Vector2 knockback = (transform.position - coll.transform.position).normalized;
            rb.isKinematic = false;
            rb.AddForce(knockback * weapon.knockback, ForceMode2D.Impulse);

            StartCoroutine(ReenableKinematicAfterKnockback());
        }
    }

    private void OnCollisionExit2D(Collision2D coll) {
        anim.speed = animSpeed;
        anim.SetBool("isAttacking", false);
        Debug.Log("stopped colliding");
        hasCollided = false;
        
    }
    private IEnumerator ReenableKinematicAfterKnockback()
    {
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }
}
