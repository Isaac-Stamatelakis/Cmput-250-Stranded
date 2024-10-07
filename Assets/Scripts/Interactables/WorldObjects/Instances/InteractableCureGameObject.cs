using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCureGameObject : InteractableGameObject<InteractableObject>
{
    public override string getInteractText()
    {
        return "Cure Your Date!";
    }

    public override void interact()
    {
        Debug.Log("TRIGGERING ENDING CUTSCENE");
    }
}
