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
            
            Player.Instance.PlayerStats.DamageDealt += damage;
            health.Damage(damage);
            NavmeshFollow navmeshFollow = collider.GetComponent<NavmeshFollow>();
            if (navmeshFollow != null)
            {
                Vector3 eulerAngles = transform.rotation.eulerAngles;
                Vector2Int knockbackDirection = AngleToDirectionVector(eulerAngles.z);
                navmeshFollow.KnockBack(knockbackDirection, weapon.knockback);
            }
            hasDamaged = true; 
        }
    }

    private Vector2Int AngleToDirectionVector(float angle)
    {
        switch (angle)
        {   
            case 0:
                return Vector2Int.right;
            case 90:
                return Vector2Int.up;
            case 180:
                return Vector2Int.left;
            case 270:
                return Vector2Int.down;
            default:
                Debug.LogWarning($"Invalid Angle {angle}");
                return Vector2Int.left;
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

        Vector3 direction = mousePosition - player.position;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            if (direction.y > 0) 
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 270);
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
