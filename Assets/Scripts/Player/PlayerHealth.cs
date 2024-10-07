using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerModule;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 100;
    [SerializeField] PlayerDeathScreenUI playerDeathScreenUIPrefab;
    [SerializeField] private PlayerUI playerUI;
    private bool dead;
    private float maxHealth;
    private int invincibleFrames = 0; // Initialize to zero

    public void Start()
    {
        this.maxHealth = health;
        if (playerUI != null)
        {
            playerUI.displayHealth(health, maxHealth);
        }
    }

    // Update is called once per frame
    public void Damage(int amount)
    {
        if (invincibleFrames > 0)
        {
            return;
        }

        invincibleFrames = 10;

        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }

        this.health -= amount;

        if (health <= 0)
        {
            health = 0;
        }

        if (playerUI != null)
        {
            playerUI.displayHealth(health, maxHealth);
        }

        if (health == 0)
        {
            Die();
        }
    }

    public void FixedUpdate()
    {
        if (invincibleFrames > 0)
        {
            invincibleFrames--;
        }
    }

    private void Die()
    {
        if (dead)
        {
            return;
        }

        dead = true;

        // Check if the death screen UI prefab and Canvas exist
        if (playerDeathScreenUIPrefab != null)
        {
            PlayerDeathScreenUI playerDeathScreenUI = GameObject.Instantiate(playerDeathScreenUIPrefab);
            GameObject canvas = GameObject.Find("Canvas");

            if (canvas != null)
            {
                playerDeathScreenUI.transform.SetParent(canvas.transform, false);
            }
            else
            {
                Debug.LogError("Canvas not found. Cannot display death screen UI.");
            }
        }
        else
        {
            Debug.LogError("PlayerDeathScreenUIPrefab is not assigned.");
        }

        // You might want to disable player movement/input here as well
        // Example: Disable the player's movement script
        // GetComponent<PlayerMovement>().enabled = false;
    }
}
