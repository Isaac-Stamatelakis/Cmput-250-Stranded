using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class InteractableCureGameObject : InteractableGameObject
{
    [SerializeField] private TimelineAsset timeline;
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
        
    }
}
