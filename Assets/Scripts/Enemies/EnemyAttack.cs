using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private int damage = 5;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the object the enemy collided with has a PlayerHealth component
        if (collider.GetComponent<PlayerHealth>() != null)
        {
            // Get the PlayerHealth component and apply damage
            PlayerHealth health = collider.GetComponent<PlayerHealth>();
            health.Damage(damage);
        }
    }
}
