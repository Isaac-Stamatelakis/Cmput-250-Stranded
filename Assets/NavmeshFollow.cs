using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;

public class NavmeshFollow : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent zom;
    private UnityEngine.AI.NavMeshObstacle obs;

    private Player player;
    private Vector3 target;
    private bool hasCollided;
    private SpriteRenderer sr;
    public int speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        zom = GetComponent<UnityEngine.AI.NavMeshAgent>();
        zom.updateRotation = false;
        zom.updateUpAxis = false;

        obs = GetComponent<UnityEngine.AI.NavMeshObstacle>();
        obs.enabled = false;
        obs.carveOnlyStationary = false;
        obs.carving = true;

        player = Player.Instance;
        hasCollided = false;

        zom.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {

        if (!hasCollided) {
            obs.enabled = false;
            obs.carving = false;
            StartCoroutine(moveTo(player.transform.position));
        } else {
            zom.enabled = false;
            obs.carving = true;
            StartCoroutine(stop());
        }

        sr.flipX = transform.position.x > player.transform.position.x;
        
        
    }

    private IEnumerator moveTo(Vector3 position) {
        yield return null;
        zom.enabled = true;
        zom.SetDestination(position);
    }

    private IEnumerator stop() {
        yield return null;
        obs.enabled = true;
        
    }

    private void goToClick() {
        if (Input.GetMouseButtonDown(0)) {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        zom.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }

    

    private void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Debug.Log("collided with player");
            hasCollided = true;
        }
    }

    private void OnCollisionExit2D(Collision2D coll) {
        Debug.Log("stopped colliding");
        hasCollided = false;
        
    }
}
