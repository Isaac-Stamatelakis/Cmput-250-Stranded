using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCureGameObject : InteractableGameObject
{
    public DatePlayer datePlayer; // Reference to the DatePlayer object

    void Start()
    {
        // Optionally, you could find the DatePlayer automatically if not assigned in the inspector.
        if (datePlayer == null)
        {
            datePlayer = FindObjectOfType<DatePlayer>();
            if (datePlayer == null)
            {
                Debug.LogError("DatePlayer object not found! Please assign it in the inspector.");
            }
        }
    }

    public override string getInteractText()
    {
        return "Cure Your Date!";
    }

    public override void interact()
    {
        Debug.Log("TRIGGERING ENDING CUTSCENE");

        // Call the Cure() method on the DatePlayer object
        if (datePlayer != null)
        {
            datePlayer.Cure(); // Trigger the Cure() method to cure the date
        }
        else
        {
            Debug.LogError("DatePlayer reference is missing! Unable to cure the date.");
        }
    }
}
