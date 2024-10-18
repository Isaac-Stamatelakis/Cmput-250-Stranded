using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;
public class MedKitObject : InteractableGameObject
{
    public override string getInteractText()
    {
        return "Heal";
    }

    public override void interact()
    {
        Player.Instance.Heal(25);
        interactable = false;
        GameObject.Destroy(gameObject);
    }
}
