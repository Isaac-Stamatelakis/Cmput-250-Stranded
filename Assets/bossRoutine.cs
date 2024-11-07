using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;

public class bossRoutine : MonoBehaviour
{
    //components
    private Player player;
    private Animator anim;
    
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

    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        anim = GetComponentInChildren<Animator>();

        //this is the main boss attack cycle
        StartCoroutine(canBossAttack());
    }

    // Update is called once per frame
    void Update()
    {
        if (canChargeAttack) {
            transform.position += (playerPos - bossPos).normalized * (chargeSpeed * Time.deltaTime);
        }

        //bounce back effect after boss hits wall
        if (bounceBack) {
            transform.position += (bossPos - playerPos).normalized * (15 * Time.deltaTime);
        }
    }

    IEnumerator canBossAttack() {
        int attack;

        while (true) {

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

            if (canAttack) {
                if (attack == 0) {

                    playerPos = player.transform.position;
                    bossPos = transform.position;

                    canChargeAttack = true;
                    canAttack = false;

                    anim.SetBool("isAboutToCharge", false);
                    anim.SetBool("isCharging", true);

                    Debug.Log("charging");
                } else {

                    Debug.Log("firing pejectiles");

                    anim.SetBool("isAboutToShoot", false);
                    anim.SetBool("isShooting", true);

                    Instantiate(projectile, transform.position + new Vector3(3.5f,0,0), Quaternion.Euler(0,0,-90));
                    Instantiate(projectile, transform.position + new Vector3(0,3.5f,0), Quaternion.Euler(0,0,0));
                    Instantiate(projectile, transform.position + new Vector3(-3.5f,0,0), Quaternion.Euler(0,0,90));
                    Instantiate(projectile, transform.position + new Vector3(0,-3.5f,0), Quaternion.Euler(0,0,-180));
                    Instantiate(projectile, transform.position + new Vector3(2.5f,2.5f,0), Quaternion.Euler(0,0,-45));
                    Instantiate(projectile, transform.position + new Vector3(-2.5f,2.5f,0), Quaternion.Euler(0,0,45));
                    Instantiate(projectile, transform.position + new Vector3(2.5f,-2.5f,0), Quaternion.Euler(0,0,-135));
                    Instantiate(projectile, transform.position + new Vector3(-2.5f,-2.5f,0), Quaternion.Euler(0,0,135));

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
