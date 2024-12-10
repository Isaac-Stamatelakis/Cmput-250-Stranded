using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerModule;
using Rooms;

public class EnemyHealth : MonoBehaviour
{
    private float maxHealth;
    [SerializeField] private int health = 20;
    [SerializeField] private int experience = 5;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite iconSprite;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private EnemyDeathBloodCollection dropBloodCollection;
    private AudioSource audioSource;
    private BossHealthBar healthBar;
    public ZombieMusicController zombieMusicController;
    private Weapon weapon;
    public BossHealthBar HealthBar => healthBar;
    public BossMusicController bossMusicController;
    private bool flashingColor = false;
    public bool isDying = false;
    public bool isBoss = false;
    public bool isBossZombies = false;
    private ParticleSystem damagedParticleSystem;

    public void Start() {
        if (spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        maxHealth = health;
        audioSource = GetComponent<AudioSource>();
        damagedParticleSystem = GetComponentInChildren<ParticleSystem>();
    }
    public void Damage(int amount)
    {
        if (healthBar != null) {
            healthBar.display(health,maxHealth,iconSprite,name);
        }
        if(amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }
        if (!flashingColor) {
            StartCoroutine(ColorOnHit());
        }

        if (damagedParticleSystem != null)
        {
            damagedParticleSystem.Play();
        }

        this.health -= amount;

        if(health <= 0 && !isDying)
        {
            Die();
        }

        if ((!isBoss && !isDying) || isBossZombies)
        {
            zombieMusicController.OnZombieHurt();
        }

    }

    public void multiplyHealth(float amount)
    {
        this.health = (int) (health * amount);
    }

    public void setHealthBar(BossHealthBar bossHealthBar) {
        this.healthBar = bossHealthBar;
        healthBar.display(health,health,iconSprite,name);
    }

    public float getHealth() {
        return health;
    }

    public float getMaxHealth() {
        return maxHealth;
    }

    private IEnumerator ColorOnHit() {
        flashingColor = true;
        Color defaultColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = defaultColor;
        flashingColor = false;
    }

    private void Die()
    {
        isDying = true;
        Player player = Player.Instance;
        player.GetComponent<PlayerLevelComponent>().AddExperience(experience);
        DateHealUpgrade dateHealUpgrade = player.DatePlayer.GetComponentInChildren<DateHealUpgrade>();
        player.PlayerStats.Kills++;
        if (isBoss && !isBossZombies)
        {
            audioSource.pitch = 0.9f;
            audioSource.PlayOneShot(deathSound);
            bossMusicController.BossDefeated();
        } else
        {
            AudioManager.Instance.PlaySound(deathSound, 0.7f, 0.9f);
        }
        if (dropBloodCollection)
        {
            GameObject blood = dropBloodCollection.GetBlood();
            blood.transform.position = transform.position;
            LevelManager.getInstance().CurrentLevel.CurrentRoom.addRoomObject(blood.transform);
        }
        if (dateHealUpgrade != null) {
            dateHealUpgrade.addKill();
        }
        EnemyDrop enemyDrop = GetComponent<EnemyDrop>();
        if (enemyDrop != null) {
            enemyDrop.DropItem();
        }
        if (isBoss)
        {
            Destroy(gameObject, deathSound != null ? deathSound.length : 0);
        } else
        {
            Destroy(gameObject);
        }
        if (healthBar != null) {
            healthBar.gameObject.SetActive(false);
        }
    }
}