using PlayerModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private int damage = 0;
    private bool hit;
    public void shootPosition(Vector2 position, int damage)
    {
        if (Player.Instance.CanMove)
        {
            this.damage = damage;
            float angle = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            StartCoroutine(MoveToPosition(position));
        }
    }

    private IEnumerator MoveToPosition(Vector2 targetPosition)
    {
        // While the current position is not equal to the target position
        while ((Vector2)transform.position != targetPosition)
        {
            // Move towards the target position
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
        yield return new WaitForSeconds(5);
        GameObject.Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hit) {
            return;
        }
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null) {
            enemyHealth.Damage(damage);
            hit = true;
        }

    }
}
