using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateAttackUpgrade : MonoBehaviour
{
    [SerializeField] private DateProjectile projectilePrefab;
    private int counter = 0;
    private int enemyLayer;
    public void Start() {
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
    }
    public void FixedUpdate() {
        counter ++;
        if (counter >= 100) {
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
        projectile.shootPosition(closestHit.transform.position,5);
    }

    
}
