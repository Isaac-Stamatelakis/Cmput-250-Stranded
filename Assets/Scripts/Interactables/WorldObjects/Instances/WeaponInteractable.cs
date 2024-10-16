using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInteractable : InteractableGameObject
{
    public Weapon weaponData;
    public override string getInteractText()
    {
        return "Grab a Weapon!";
    }

    public override void interact()
    {
        PlayerAttack playerAttack = FindObjectOfType<PlayerAttack>();

        playerAttack.SetWeapon(weaponData);
        interactable = false;
        GameObject.Destroy(gameObject);
    }
}
