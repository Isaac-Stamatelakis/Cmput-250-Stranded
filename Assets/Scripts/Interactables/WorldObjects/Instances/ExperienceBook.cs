using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;
using Rooms;

public class ExperienceBook : InteractableGameObject
{
    [SerializeField] private int experience = 25;
    public override string getInteractText()
    {
        bool clear = LevelManager.getInstance().CurrentLevel.CurrentRoomClear();
        if (clear) {
            return $"<color=green>Gather Knowledge</color>";
        } else {
            return "<color=red>Cannot Gather Knowledge\nNot a Safe Learning Environment</color>";
        }

    }

    public override void interact()
    {
        bool clear = LevelManager.getInstance().CurrentLevel.CurrentRoomClear();
        if (!clear) {
            return;
        }
        Player.Instance.GetComponent<PlayerLevelComponent>().AddExperience(experience);
        interactable = false;
        GameObject.Destroy(gameObject);
    }
}
