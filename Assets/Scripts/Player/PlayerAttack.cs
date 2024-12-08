using UnityEngine;
using Dialogue;
using PlayerModule;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackArea;
    public Animator playerAnimator;
    public bool attacking = false;          
    private bool attackPerformed = false;  
    public Weapon currentWeapon;           
    private float timer = 0f;             
    public PlayerAttackSFX attackSFX;           
    private RuntimeAnimatorController originalAnimatorController;
    private ParticleSystem particleSystem;
    private int attackLayerIndex;

    void Start()
    {
        originalAnimatorController = playerAnimator.runtimeAnimatorController;
        attackLayerIndex = playerAnimator.GetLayerIndex("Attack Layer");
        particleSystem = GetComponentInChildren<ParticleSystem>();

    }

    void Update()
    {
        if (!DialogUIController.Instance.ShowingDialog && Input.GetMouseButtonDown(0) && !attackPerformed)
        {
            Attack();
        }

        if (attacking)
        {
            timer += Time.deltaTime;

            if (timer >= currentWeapon.attackTime)
            {
                EndAttack();
            }
        }
    }

    private void Attack()
    {
        if (attacking || currentWeapon == null)
        {
            return;
        }

        playerAnimator.SetLayerWeight(attackLayerIndex, 1f);
        attacking = true;                     
        attackPerformed = true;               
        attackArea.SetActive(true);          
        attackSFX.PlaySound(currentWeapon,AttackSFX.Swing);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        
        Vector3 direction = mousePosition - transform.position;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x < 0)
            {
                playerAnimator.SetTrigger("AttackLeft");
                Debug.Log("Attacking Left");
            }
            else
            {
                playerAnimator.SetTrigger("AttackRight");
                Debug.Log("Attacking Right");
            }
        }
        else
        {
            if (direction.y > 0)
            {
                playerAnimator.SetTrigger("AttackUp");
                Debug.Log("Attacking up");
            }
            else
            {
                playerAnimator.SetTrigger("AttackDown");
                Debug.Log("Attacking down");
            }
        }
        
        //particleSystem.transform.Translate(0.5f * dir, 0, 0);

        AttackArea attackAreaScript = attackArea.GetComponent<AttackArea>();
        if (attackAreaScript != null)
        {
            attackAreaScript.SetWeapon(currentWeapon);
        }

        playerAnimator.SetBool("isAttacking", true);
    }

    private void EndAttack()
    {
        attacking = false;
        attackPerformed = false;
        playerAnimator.SetBool("isAttacking", false);
        attackArea.SetActive(false);
        bool attackedLeft = particleSystem.transform.localPosition.x > 0;
        int dir = attackedLeft ? -1 : 1;
        particleSystem.transform.Translate(0.5f * dir, 0, 0);
        AttackArea attackAreaScript = attackArea.GetComponent<AttackArea>();
        if (attackAreaScript != null)
        {
            attackAreaScript.ResetDamageFlag();
        }

        playerAnimator.SetLayerWeight(attackLayerIndex, 0f);
        timer = 0f; 
    }

    public void SetWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;

        if (newWeapon != null && newWeapon.weaponAnimatorOverride != null)
        {
            playerAnimator.runtimeAnimatorController = newWeapon.weaponAnimatorOverride;
        }
        else
        {
            playerAnimator.runtimeAnimatorController = originalAnimatorController;
        }
        WeaponStatsUI weaponStatsUI = FindObjectOfType<WeaponStatsUI>();
        WeaponStatsUI.Instance?.UpdateWeaponStats(currentWeapon);
    }
}
