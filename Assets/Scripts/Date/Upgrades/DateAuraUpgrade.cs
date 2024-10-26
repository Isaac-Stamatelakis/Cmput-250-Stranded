using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;

public class DateAuraUpgrade : MonoBehaviour
{
    private int playerLayer;
    private PlayerLevelComponent playerLevelComponent;
    public void Start() {
        playerLayer = 1 << LayerMask.NameToLayer("Player");
        playerLevelComponent = Player.Instance.GetComponent<PlayerLevelComponent>();

    }
    public void FixedUpdate() {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 3, Vector2.zero, Mathf.Infinity, playerLayer);
        playerLevelComponent.DateAura = hit.collider != null;
    }
}
