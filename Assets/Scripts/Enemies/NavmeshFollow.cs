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
    private bool isKnockedBack;
    

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
        //rb.isKinematic = true;

        player = Player.Instance;
        hasCollided = false;

        zom.speed = speed;
    }

    public void KnockBack(Vector2Int direction, float force)
    {
        if (!isKnockedBack)
        {
            StartCoroutine(KnockbackCoroutine(direction, force, 0.1f));
        }
    }
    private IEnumerator KnockbackCoroutine(Vector2Int direction, float force, float duration)
    {
        if (!zom.isOnNavMesh)
        {
            yield break;
        }
        isKnockedBack = true;
        zom.isStopped = true;
        
        Vector2 forceDir = force * (Vector2)direction;
        rb.AddForce(forceDir, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(duration);
        
        zom.isStopped = false;
        isKnockedBack = false;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isWalking",!hasCollided);
        StartCoroutine(moveTo(player.transform.position));
        
        /*
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
        */

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
        if (coll.gameObject.CompareTag("Player")) {
            anim.speed = animSpeed * 1.25f;
            anim.SetBool("isAttacking", true);
            hasCollided = true;
            //Vector2 dir = (transform.position - coll.transform.position).normalized;
            //Vector2 forceDir = dir.x > 0 ? Vector2.left : Vector2.right;
            //Rigidbody2D rb = coll.gameObject.GetComponent<Rigidbody2D>();
            //rb.AddForce(forceDir*25,ForceMode2D.Impulse);
            
            //Vector2 knockback = (transform.position+ sr.bounds.center - (coll.transform.position+coll.gameObject.GetComponent<SpriteRenderer>().bounds.center)).normalized;
            
            //Debug.Log(knockback);
            //Rigidbody2D rb = coll.gameObject.GetComponent<Rigidbody2D>();
            //rb.AddForce(knockback*1000,ForceMode2D.Impulse);

            //StartCoroutine(ReenableKinematicAfterKnockback());
        }
    }

    private void OnCollisionExit2D(Collision2D coll) {
        anim.speed = animSpeed;
        anim.SetBool("isAttacking", false);
        hasCollided = false;
        
    }
    private IEnumerator ReenableKinematicAfterKnockback()
    {
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
        //rb.isKinematic = true;
    }
}
