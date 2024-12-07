using System;
using System.Collections;
using System.Collections.Generic;
using PlayerModule;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class BossKnockBack : MonoBehaviour
{
    public int Knockback;
    private Animator animator;

    public void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PLAYER_MOVE_TEST playerMove = other.gameObject.GetComponent<PLAYER_MOVE_TEST>();
        if (playerMove != null)
        {
            Vector2 knockbackDirection = other.transform.position - transform.position;
            knockbackDirection.Normalize();
            float currentKnockBack = Knockback;
            if (animator.GetBool("isCharging"))
            {
                int ran = UnityEngine.Random.Range(0, 2);
                if (ran == 0)
                {
                    ran = -1;
                }
                knockbackDirection = RotateVector(knockbackDirection, 90*ran);
                currentKnockBack *= 2;
            }
            
            playerMove.ApplyKnockback(knockbackDirection*currentKnockBack);
        }
        
    }
    
    private Vector2 RotateVector(Vector2 vector, float angle)
    {
        float radianAngle = Mathf.Deg2Rad * angle;
        float cosAngle = Mathf.Cos(radianAngle);
        float sinAngle = Mathf.Sin(radianAngle);
        float newX = cosAngle * vector.x - sinAngle * vector.y;
        float newY = sinAngle * vector.x + cosAngle * vector.y;

        return new Vector2(newX, newY);
    }
    
}
