using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerModule;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 20;
    [SerializeField] private float experience = 5;
    private bool flashingColor = false;

    public void Damage(int amount)
    {
        if(amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }
        if (!flashingColor) {
            StartCoroutine(ColorOnHit());
        }
        
        this.health -= amount;

        if(health <= 0)
        {
            Die();
        }
    }

    private IEnumerator ColorOnHit() {
        flashingColor = true;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        flashingColor = false;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}