using System.Collections;
using System.Collections.Generic;
using PlayerModule;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    private void OnCollisionStay2D(Collision2D collider)
    {
        // Check if the object the enemy collided with has a PlayerHealth component
        if (collider.gameObject.GetComponent<PlayerHealth>() != null)
        {
            // Get the PlayerHealth component and apply damage
            PlayerHealth health = collider.gameObject.GetComponent<PlayerHealth>();
            Player player = Player.Instance;
            player.PlayerStats.DamageTaken += damage;
            health.Damage(damage);
        }
    }
}
