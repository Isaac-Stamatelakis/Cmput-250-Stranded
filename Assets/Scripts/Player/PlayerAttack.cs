using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackArea;
    public Animator playerAnimator;
    private bool attacking = false;
    private bool attackPerformed = false;
    public Weapon currentWeapon;
    private float timer = 0f;
    public AudioSource audioSource;
    private RuntimeAnimatorController originalAnimatorController;
    void Start()
    {
        originalAnimatorController = playerAnimator.runtimeAnimatorController;
        audioSource = GetComponentInChildren<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !attackPerformed)
        {
            Attack();
        }

        if (attacking)
        {
            timer += Time.deltaTime;

            if (timer >= currentWeapon.attackTime)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(false);

                AttackArea attackAreaScript = attackArea.GetComponent<AttackArea>();
                if (attackAreaScript != null)
                {
                    attackAreaScript.ResetDamageFlag();
                }
                attackPerformed = false;
            }
        }
    }

    private void Attack()
    {
        if (playerAnimator.runtimeAnimatorController == originalAnimatorController)
        {
            return; 
        }
        attacking = true;
        attackPerformed = true;
        attackArea.SetActive(true);
        audioSource.PlayOneShot(currentWeapon.swingSound);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        if (mousePosition.x < transform.position.x)
        {
            playerAnimator.SetTrigger("AttackLeft");
        }
        else
        {
            playerAnimator.SetTrigger("AttackRight");
        }

        AttackArea attackAreaScript = attackArea.GetComponent<AttackArea>();
        if (attackAreaScript != null)
        {
            attackAreaScript.SetWeapon(currentWeapon);
        }
    }

    
    public void SetWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;

        
        if (newWeapon.weaponAnimatorOverride != null)
        {
            playerAnimator.runtimeAnimatorController = newWeapon.weaponAnimatorOverride;
        }
        else
        {
            
            playerAnimator.runtimeAnimatorController = originalAnimatorController;
        }
    }
}
