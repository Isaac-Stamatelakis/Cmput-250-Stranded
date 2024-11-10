using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;
public class MedKitObject : InteractableGameObject
{
    public override string getInteractText()
    {
        PlayerHealth playerHealth = Player.Instance.GetComponent<PlayerHealth>();
        if (playerHealth.isFull()) {
            return "<color=red>Can't Heal\nToo Healthy</color>";
        }
        return "<color=green>Heal</color>";
    }

    public override void interact()
    {
        PlayerHealth playerHealth = Player.Instance.GetComponent<PlayerHealth>();
        if (playerHealth.isFull()) {
            return;
        }
        Player.Instance.Heal(50);
        interactable = false;
        GameObject.Destroy(gameObject);
    }
}
