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
    public void FixedUpdate()
    {
        float range = PlayerUpgradeUtils.DATE_AURA_RANGE;
        if (playerLevelComponent.HasUpgrade(PlayerUpgrade.AngryRock))
        {
            range *= 1.5f;
        }
        RaycastHit2D hit = Physics2D.CircleCast(transform.position,range, Vector2.zero, Mathf.Infinity, playerLayer);
        playerLevelComponent.SetDateAura(hit.collider != null);
    }
}
