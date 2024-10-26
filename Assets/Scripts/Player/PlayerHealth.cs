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
    [SerializeField] private PlayerHurtSFX playerHurtSFX;
    private bool dead;
    private float maxHealth;
    private int invincibleFrames = 0; // Initialize to zero
    public float Health => health;
    public float MaxHealth => maxHealth;

    public void Start()
    {
        // If max health is zero then player has not been unseraialized from data
        bool initalized = maxHealth > 0;
        if (!initalized) {
            setHealth(health);
        }

    }

    public void setHealth(float currentHealth) {
        this.maxHealth = this.health;
        this.health = currentHealth;
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
        PlayerLevelComponent playerLevelComponent = GetComponent<PlayerLevelComponent>();
        if (playerLevelComponent.hasUpgrade(PlayerUpgrade.DamageReduction)) {
            amount = Mathf.FloorToInt(amount * PlayerUpgradeUtils.DAMAGE_REDUCTION_MODIFIER);
        }
        if (playerLevelComponent.DateAura) {
            amount = Mathf.FloorToInt(amount * PlayerUpgradeUtils.DAMAGE_REDUCTION_MODIFIER);
        }
        this.health -= amount;

        if (health <= 0)
        {
            health = 0;
        } else {
            playerHurtSFX.PlaySound(PlayerHurtSound.Damaged);
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

    public void Heal(float amount) {
        this.health += amount;
        if (health > maxHealth) {
            health = maxHealth;
        }
        playerUI.displayHealth(health,maxHealth);
    }

    public void IncreaseHealth(float amount) {
        maxHealth += amount;
        Heal(amount);
    }
    public void FixedUpdate() {
        if (invincibleFrames > 0) {
            invincibleFrames --;
        }
    }

    private void Die()
    {
        if (dead)
        {
            return;
        }

        dead = true;
        playerHurtSFX.PlaySound(PlayerHurtSound.Death);

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
