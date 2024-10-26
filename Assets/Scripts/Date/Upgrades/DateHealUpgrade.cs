using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;

public class DateHealUpgrade : MonoBehaviour
{
    private int counter;
    public void addKill() {
        counter ++;
        if (counter >= PlayerUpgradeUtils.DATE_KILL_HEAL_COUNT) {
            healPlayer();
            counter = 0;
        }
    }

    public void healPlayer() {
        Player.Instance.Heal(PlayerUpgradeUtils.DATE_KILL_HEAL_AMOUNT);
    }
}
