using System;
using System.Collections;
using System.Collections.Generic;
using PlayerModule;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionStay2D(Collision2D collider)
    {
        
        // Check if the object the enemy collided with has a PlayerHealth component
        if (collider.gameObject.GetComponent<PlayerHealth>() != null)
        {
            if (animator != null)
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                float normalizedTime = stateInfo.normalizedTime;

                // If the animation is looped, subtract the integer part to get the fractional time
                if (normalizedTime > 1)
                {
                    normalizedTime -= Mathf.Floor(normalizedTime);
                }
                float animationTime = normalizedTime * stateInfo.length;
                if (animationTime < 0.1f)
                {
                    return;
                }
            }
            PlayerHealth health = collider.gameObject.GetComponent<PlayerHealth>();
            EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
            if (!enemyHealth.isDying)
            {
                Player player = Player.Instance;
                player.PlayerStats.DamageTaken += damage;
                health.Damage(damage);
            }
        }
    }
}
