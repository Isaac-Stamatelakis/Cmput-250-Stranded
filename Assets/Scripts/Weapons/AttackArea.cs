using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public Transform player; 
    public float attackRange = 2.0f;
    private int damage = 5;

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 
        Vector3 direction = (mousePosition - player.position).normalized;
        transform.position = player.position + direction * attackRange;

        transform.up = direction;
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<EnemyHealth>() != null)
        {
            EnemyHealth health = collider.GetComponent<EnemyHealth>();
            health.Damage(damage);
        }
    }
}
