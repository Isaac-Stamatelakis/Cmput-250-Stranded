using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerModule;

public class EnemyHealth : MonoBehaviour
{
    private float maxHealth;
    [SerializeField] private int health = 20;
    [SerializeField] private int experience = 5;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite iconSprite;
    [SerializeField] private AudioClip deathSound;
    private AudioSource audioSource;
    private BossHealthBar healthBar;
    private bool flashingColor = false;
    public bool isDying = false;

    public void Start() {
        if (spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        maxHealth = health;
        audioSource = GetComponent<AudioSource>();
    }
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

        if(health <= 0 && !isDying)
        {
            Die();
        }
        if (healthBar != null) {
            healthBar.display(health,maxHealth,iconSprite,name);
        }
    }

    public void setHealthBar(BossHealthBar bossHealthBar) {
        this.healthBar = bossHealthBar;
        healthBar.display(health,health,iconSprite,name);
    }

    private IEnumerator ColorOnHit() {
        flashingColor = true;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        flashingColor = false;
    }

    private void Die()
    {
        isDying = true;
        audioSource.PlayOneShot(deathSound);
        Player.Instance.GetComponent<PlayerLevelComponent>().addExperience(experience);
        DateHealUpgrade dateHealUpgrade = Player.Instance.DatePlayer.GetComponentInChildren<DateHealUpgrade>();
        if (dateHealUpgrade != null) {
            dateHealUpgrade.addKill();
        }
        EnemyDrop enemyDrop = GetComponent<EnemyDrop>();
        if (enemyDrop != null) {
            enemyDrop.DropItem();
        }
        Destroy(gameObject, deathSound != null ? deathSound.length : 0);
        if (healthBar != null) {
            healthBar.gameObject.SetActive(false);
        }
    }
}