using System.Collections;
using System.Collections.Generic;
using PlayerModule;
using Rooms;
using UnityEngine;

public class DateAttackUpgrade : MonoBehaviour
{
    [SerializeField] private DateProjectile projectilePrefab;
    private PlayerLevelComponent playerLevelComponent;
    private int counter = 0;
    private int enemyLayer;
    public void Start() {
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
        playerLevelComponent = Player.Instance.GetComponent<PlayerLevelComponent>();
    }
    public void FixedUpdate() {
        counter ++;
        if (playerLevelComponent.HasUpgrade(PlayerUpgrade.Attack))
        {
            counter ++;
        }
        if (counter >= 50*PlayerUpgradeUtils.DATE_ATTACK_RATE) {
            attack();
            counter = 0;
        }
    }
    private void attack() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 10, Vector2.zero, Mathf.Infinity, enemyLayer);
        if (hits.Length == 0) {
            return;
        }
        RaycastHit2D closestHit = hits[0];
        for (int i = 1; i < hits.Length; i++) {
            if (hits[i].distance < closestHit.distance) {
                closestHit = hits[i];
            }
        }
        DateProjectile projectile = GameObject.Instantiate(projectilePrefab);
        projectile.transform.position = transform.position;
        projectile.shootPosition(closestHit.transform.position,PlayerUpgradeUtils.DATE_ATTACK_DAMAGE);
    }

    
}
