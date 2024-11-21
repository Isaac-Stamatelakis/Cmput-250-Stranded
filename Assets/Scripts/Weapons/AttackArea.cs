using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;

public class AttackArea : MonoBehaviour
{
    public Transform player;  
    public Weapon weapon;
    private PolygonCollider2D polygonCollider;
    private PlayerLevelComponent playerLevelComponent;
    private bool hasDamaged = false;
    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        SetColliderPointsBasedOnWeapon();
        playerLevelComponent = Player.Instance.GetComponent<PlayerLevelComponent>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (weapon == null) {
            return;
        }
        if (!hasDamaged && collider.GetComponent<EnemyHealth>() != null)
        {
            
            EnemyHealth health = collider.GetComponent<EnemyHealth>();
            if (health == null) {
                return;
            }
            int damage = weapon.damage;
            if (playerLevelComponent.HasUpgrade(PlayerUpgrade.Attack)) {
                damage = (int) (damage * PlayerUpgradeUtils.DAMAGE_UPGRADE_MODIFIER);
                if (playerLevelComponent.HasUpgrade(PlayerUpgrade.AngryRock))
                {
                    damage = (int) (damage * PlayerUpgradeUtils.DAMAGE_UPGRADE_MODIFIER);
                }
            }
            if (playerLevelComponent.DateAura) {
                damage = (int) (damage * PlayerUpgradeUtils.DAMAGE_UPGRADE_MODIFIER);
                if (playerLevelComponent.HasUpgrade(PlayerUpgrade.AngryRock))
                {
                    damage = (int) (damage * PlayerUpgradeUtils.DAMAGE_UPGRADE_MODIFIER);
                }
            }
            
            health.Damage(damage);
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

        // Set direction of attack
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        bool left = mousePosition.x < player.position.x;
        if (left) {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        } else { // right
            transform.rotation = Quaternion.Euler(0, 0, 0);
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
