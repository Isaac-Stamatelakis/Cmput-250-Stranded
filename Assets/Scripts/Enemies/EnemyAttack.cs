using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private int damage = 5;

    private void OnCollisionStay2D(Collision2D collider)
    {
        // Check if the object the enemy collided with has a PlayerHealth component
        if (collider.gameObject.GetComponent<PlayerHealth>() != null)
        {
            // Get the PlayerHealth component and apply damage
            PlayerHealth health = collider.gameObject.GetComponent<PlayerHealth>();
            health.Damage(damage);
        }
    }
}
