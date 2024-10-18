using UnityEngine;
using Dialogue;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackArea;
    public Animator playerAnimator;
    public bool attacking = false;          
    private bool attackPerformed = false;  
    public Weapon currentWeapon;           
    private float timer = 0f;             
    public AudioSource audioSource;           
    private RuntimeAnimatorController originalAnimatorController;
    private int attackLayerIndex;

    void Start()
    {
        originalAnimatorController = playerAnimator.runtimeAnimatorController;
        audioSource = GetComponentInChildren<AudioSource>();
        attackLayerIndex = playerAnimator.GetLayerIndex("Attack Layer");

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
        audioSource.PlayOneShot(currentWeapon.swingSound);  

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        if (mousePosition.x < transform.position.x)
        {
            playerAnimator.SetTrigger("AttackLeft");
            Debug.Log("Attacking Left");
        }
        else
        {
            playerAnimator.SetTrigger("AttackRight");
            Debug.Log("Attacking Right");
        }

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
    }
}
