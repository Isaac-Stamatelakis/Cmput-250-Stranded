using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public Transform player;  
    public Weapon weapon;
    private PolygonCollider2D polygonCollider;

    private bool hasDamaged = false;
    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        SetColliderPointsBasedOnWeapon();
    }

    void Update()
    {
        if (weapon == null) return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        
        if (mousePosition.x < player.position.x)
        {
            
            transform.position = player.position + Vector3.left * weapon.range;
        }
        else
        {
            
            transform.position = player.position + Vector3.right * weapon.range;
        }
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

    public void SetColliderPointsBasedOnWeapon()
    {
        if (polygonCollider != null && weapon != null)
        {
            
            Vector2[] points = polygonCollider.points;

            if (points.Length > 0)
            {
                points[0] = new Vector2(weapon.range, 0);
                polygonCollider.points = points;
            }
        }
    }



    public void SetWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;
        SetColliderPointsBasedOnWeapon();
    }

    
    public void ResetDamageFlag()
    {
        hasDamaged = false;
    }
}
