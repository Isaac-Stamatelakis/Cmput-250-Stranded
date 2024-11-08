using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class InteractableCureGameObject : InteractableGameObject
{
    [SerializeField] private TimelineAsset timeline;
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
        return "<color=green>Cure Your Date!</color>";
    }

    public override void interact()
    {
        Debug.Log("TRIGGERING ENDING CUTSCENE");
        GameObject director = new GameObject();
        director.name = "director";
        PlayableDirector playableDirector = director.AddComponent<PlayableDirector>();
        playableDirector.playableAsset = timeline;
        
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
