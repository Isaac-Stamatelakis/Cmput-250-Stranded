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

        if (DialogueState.IsDialogueActive)
        {
            StopEnemyAnimation(); // Stop animations during dialogue
            return;
        }

        ResumeEnemyAnimation();
        // Prevent attacks while dialogue is active
        if (DialogueState.IsDialogueActive) return;

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
            Player player = Player.Instance;
            player.PlayerStats.DamageTaken += damage;
            health.Damage(damage);
        }
    }

    private void StopEnemyAnimation()
    {
        if (animator != null)
        {
            animator.speed = 0; // Freeze animation
        }
    }

    private void ResumeEnemyAnimation()
    {
        if (animator != null)
        {
            animator.speed = 1; // Resume animation
        }
    }
}