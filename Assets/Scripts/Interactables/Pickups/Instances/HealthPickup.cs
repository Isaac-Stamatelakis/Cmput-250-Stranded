using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;

public class HealthPickup : PickupObject
{
    protected override void onPickup()
    {
        Player.Instance.Heal(5);
    }
}
