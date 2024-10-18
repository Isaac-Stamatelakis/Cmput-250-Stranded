using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInteractable : InteractableGameObject
{
    public Weapon weaponData;
    public override string getInteractText()
    {
        return $"Pick up {weaponData.name}!";
    }

    public override void interact()
    {
        PlayerAttack playerAttack = FindObjectOfType<PlayerAttack>();

        Weapon playerWeapon = playerAttack.currentWeapon;
        playerAttack.SetWeapon(weaponData);
        weaponData = playerWeapon;
        if (weaponData == null) {
            interactable = false;
            GameObject.Destroy(gameObject);
        } else {
            GetComponent<SpriteRenderer>().sprite = weaponData.artwork;
        }
    }
}
