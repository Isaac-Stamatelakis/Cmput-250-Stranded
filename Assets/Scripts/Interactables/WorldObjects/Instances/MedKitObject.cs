using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;
public class MedKitObject : InteractableGameObject<InteractableObject>
{
    public override string getInteractText()
    {
        return "Heal";
    }

    public override void interact()
    {
        Player.Instance.Heal(5);
        interactable = false;
        GameObject.Destroy(gameObject);
    }
}
