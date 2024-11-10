using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class InteractableCureGameObject : InteractableGameObject
{
    [SerializeField] private GameObject timeLineAsset;
    public DatePlayer datePlayer; // Reference to the DatePlayer object

    public override string getInteractText()
    {
        return "<color=green>Cure Your Date!</color>";
    }

    public override void interact()
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
        Debug.Log("TRIGGERING ENDING CUTSCENE");


        // Call the Cure() method on the DatePlayer object
        if (datePlayer != null)
        {
            StartCoroutine(cureRoutine());
        }
        else
        {
            Debug.LogError("DatePlayer reference is missing! Unable to cure the date.");
        }

    
    }

    private IEnumerator cureRoutine() {
        interactable = false;
        GetComponent<SpriteRenderer>().enabled = false;
        yield return StartCoroutine(datePlayer.Cure());
        GameObject.Destroy(gameObject);
    }
}
