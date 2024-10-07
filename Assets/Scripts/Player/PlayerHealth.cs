using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerModule;
using PlayerModule;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 100;
    [SerializeField] PlayerDeathScreenUI playerDeathScreenUIPrefab;
    [SerializeField] private PlayerUI playerUI;
    private bool dead;
    private float maxHealth;
    public void Start() {
        this.maxHealth = health;
        playerUI.displayHealth(health,maxHealth);
    }
    private int invincibleFrames;

    // Update is called once per frame
    public void Damage(int amount)
    {
        if (invincibleFrames > 0) {
            return;
        }
        invincibleFrames = 10;
        if(amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }
        this.health -= amount;
        if (health <= 0) {
            health = 0;
        }
        playerUI.displayHealth(health,maxHealth);
        if(health == 0)
        {
            Die();
        }
    }

    public void Heal(float amount) {
        this.health += amount;
        if (health > maxHealth) {
            health = maxHealth;
        }
    }
    public void FixedUpdate() {
        if (invincibleFrames > 0) {
            invincibleFrames --;
        }
        
    }

    private void Die()
    {
        if (dead) {
            return;
        }
        dead = true;
        PlayerDeathScreenUI playerDeathScreenUI = GameObject.Instantiate(playerDeathScreenUIPrefab);
        playerDeathScreenUI.transform.SetParent(GameObject.Find("Canvas").transform,false);
    }
}