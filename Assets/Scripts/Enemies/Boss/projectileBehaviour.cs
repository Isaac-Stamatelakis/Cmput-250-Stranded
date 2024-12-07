using System.Collections;
using System.Collections.Generic;
using PlayerModule;
using UnityEngine;

public class projectileBehaviour : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int projectileDamage = 15;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.up * speed;
    }

    void OnCollisionEnter2D (Collision2D col) {
        if (col.gameObject.name != "projectile(Clone)") {
            Debug.Log("bullet hit");

            if (col.gameObject.GetComponent<PlayerHealth>() != null)
            {
                // Get the PlayerHealth component and apply damage
                PlayerHealth health = col.gameObject.GetComponent<PlayerHealth>();
                health.Damage(projectileDamage);
                Vector3 eulerAngles = transform.rotation.eulerAngles;
                float angle = eulerAngles.z + 90f;
                Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad *angle), Mathf.Sin(Mathf.Deg2Rad * angle));
                col.gameObject.GetComponent<PLAYER_MOVE_TEST>().ApplyKnockback(direction*150);
                Debug.Log("damge player");
            }

            Destroy(this.gameObject);
        }
    }
}
