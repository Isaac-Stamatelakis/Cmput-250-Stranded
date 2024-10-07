using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackArea = default;
    public Image weaponImage;

    private bool attacking = false;
    private bool attackPerformed = false;
    public Weapon weapon;
    private float timer = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !attackPerformed)
        {
            Attack();
        }

        if (attacking)
        {
            timer += Time.deltaTime;

            if (timer >= weapon.attackTime)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(false);
                weaponImage.gameObject.SetActive(false);

                
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
        attacking = true;
        attackPerformed = true;
        attackArea.SetActive(true);

        weaponImage.sprite = weapon.artwork;
        weaponImage.gameObject.SetActive(true);
    }
}
