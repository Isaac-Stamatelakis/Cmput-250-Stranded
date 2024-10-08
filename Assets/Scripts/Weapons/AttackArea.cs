using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class AttackArea : MonoBehaviour
{
    public Transform player;
    public Image weaponImage;

    public Weapon weapon;

    private bool hasDamaged = false; 

    void Update()
    {
      
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 direction = (mousePosition - player.position).normalized;
        transform.position = player.position + direction * weapon.range;
        transform.up = direction;
        weaponImage.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (!hasDamaged && collider.GetComponent<EnemyHealth>() != null)
        {
            EnemyHealth health = collider.GetComponent<EnemyHealth>();
            health.Damage(weapon.damage);
            hasDamaged = true; 
        }
    }

    
    public void ResetDamageFlag()
    {
        hasDamaged = false;
    }
}
