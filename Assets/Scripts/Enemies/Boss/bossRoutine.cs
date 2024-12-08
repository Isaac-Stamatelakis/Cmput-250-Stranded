using System.Collections;
using System.Collections.Generic;
using Difficulty;
using UnityEngine;
using PlayerModule;
using Rooms;

public class bossRoutine : MonoBehaviour
{
    //components
    private Player player;
    private Animator anim;
    private SpriteRenderer spr;
    private AudioSource audioSource;
    //static positions for charge attack
    private Vector3 playerPos;
    private Vector3 bossPos;
    
    //bool triggers for what attack the boss is doing or can do
    private bool canChargeAttack = false;
    private bool canAttack = true;
    private bool bounceBack = false;

    //changeable stuff, kinda not projectile
    [SerializeField] private float chargeSpeed = 40;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float attackSpeed = 2f;
    [SerializeField] private GameObject BossZombies;
    public AudioClip chargeAttackSound;
    public AudioClip projectileAttackSound;
    public EnemyHealth enemyHealth;

    //stuff related for half health stuff
    private bool halfHealth = false;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyHealth.isBoss = true;
        player = Player.Instance;
        anim = GetComponentInChildren<Animator>();
        spr = GetComponentInChildren<SpriteRenderer>();
        enemyHealth = GetComponent<EnemyHealth>();
        audioSource = GetComponent<AudioSource>();

        //this is the main boss attack cycle
        StartCoroutine(canBossAttack());
        DifficultyModifier modifier = LevelManager.getInstance().DifficultyModifier;
        float healthModifier = modifier.GetBossHealthModifier();
        float speedModifier = modifier.GetBossSpeedModifier();
        enemyHealth.multiplyHealth(healthModifier);
        attackSpeed *= speedModifier;
        BossHealthBar bossHealthBar = BossHealthBar.Instance;
        enemyHealth.setHealthBar(bossHealthBar);
        bossHealthBar.gameObject.SetActive(true);
        

        //Debug.Log($"{totalHealth}");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"{enemyHealth.getMaxHealth()}");
        Debug.Log($"{enemyHealth.getHealth()}");
        if (canChargeAttack) {
            transform.position += (playerPos - bossPos).normalized * (chargeSpeed * Time.deltaTime);
        }

        //bounce back effect after boss hits wall
        if (bounceBack) {
            transform.position += (bossPos - playerPos).normalized * (15 * Time.deltaTime);
        }
        Vector3 position = transform.position;
        position.z = -2;
        transform.position = position;

        Debug.Log($"is half: {(enemyHealth.getHealth() < (enemyHealth.getMaxHealth()/2)) && !halfHealth}");
        if ((enemyHealth.getHealth() < (enemyHealth.getMaxHealth()/2)) && !halfHealth) {
            //Debug.Log(spr==null);
            Debug.Log("spawning zmobies");
            Debug.Log($"is it set: {BossZombies}");
            BossZombies.SetActive(true);
            halfHealth = true;
        }
    }

    IEnumerator canBossAttack() {
        int attack;

        while (!enemyHealth.isDying) {

            Debug.Log("waiting");

            anim.SetBool("isIdle", true);
            yield return new WaitForSeconds(attackSpeed);
            anim.SetBool("isIdle", false);

            //decide attack
            attack = Random.Range(0,2);

            if (attack == 0) {
                anim.SetBool("isAboutToCharge", true);
            } else {
                anim.SetBool("isAboutToShoot", true);
            }
            yield return new WaitForSeconds(1);

            if (canAttack && !enemyHealth.isDying) {
                if (attack == 0) {
                    audioSource.pitch = 0.8f;
                    audioSource.volume = 0.8f;
                    audioSource.PlayOneShot(chargeAttackSound);

                    playerPos = player.transform.position;
                    bossPos = transform.position;
                    

                    canChargeAttack = true;
                    canAttack = false;

                    anim.SetBool("isAboutToCharge", false);
                    anim.SetBool("isCharging", true);

                    Debug.Log("charging");
                } else {

                    Debug.Log("firing pejectiles");
                    audioSource.pitch = Random.Range(0.4f, 0.5f);
                    audioSource.PlayOneShot(projectileAttackSound);
                    anim.SetBool("isAboutToShoot", false);
                    anim.SetBool("isShooting", true);
                    
                    int ran = UnityEngine.Random.Range(0,2);
                    float startAngle = 0;
                    if (ran == 1) {
                        startAngle = 22.5f;
                    } 
                    Vector3[] positions = new Vector3[]
                    {
                        new Vector3(3.5f, 0, 0),
                        new Vector3(0, 3.5f, 0),
                        new Vector3(-3.5f, 0, 0),
                        new Vector3(0, -3.5f, 0),
                        new Vector3(2.5f, 2.5f, 0),
                        new Vector3(-2.5f, 2.5f, 0),
                        new Vector3(2.5f, -2.5f, 0),
                        new Vector3(-2.5f, -2.5f, 0)
                    };

                    float[] rotations = new float[] { -90, 0, 90, -180, -45, 45, -135, 135 };

                    for (int i = 0; i < rotations.Length; i++)
                    {
                        Instantiate(projectile, transform.position + positions[i], Quaternion.Euler(0, 0, rotations[i]+startAngle));
                    }

                    yield return new WaitForSeconds(0.5f);
                    anim.SetBool("isShooting", false);
                }
            }
        }
    }

    IEnumerator finishChargeAttack() { //this decides how long the bounce back is
        canChargeAttack = false;
        bounceBack = true;

        yield return new WaitForSeconds(.1f);

        bounceBack = false;
        canAttack = true;
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.name == "WallTileMap") {

            StartCoroutine(finishChargeAttack());
            
            Debug.Log("collided with wall");
            anim.SetBool("isCharging", false);
        }
    }
}
