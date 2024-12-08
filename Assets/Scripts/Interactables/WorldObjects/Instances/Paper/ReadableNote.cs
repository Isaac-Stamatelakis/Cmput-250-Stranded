using System.Collections;
using System.Collections.Generic;
using Rooms;
using UnityEngine;

public class ReadableNote : InteractableGameObject
{
    [SerializeField, TextArea(3, 3)] public string Header;
    [SerializeField, TextArea(10, 10)] public string Content;
    public override void interact()
    {
        bool clear = LevelManager.getInstance().CurrentLevel.CurrentRoomClear();
        if (!clear)
        {
            return;
        }
        ReadNoteUIController.Instance.Display(Header,Content);
    }

    public override string getInteractText()
    {
        bool clear = LevelManager.getInstance().CurrentLevel.CurrentRoomClear();
        return !clear ? "<color=red>Not a Safe Reading Environment</color>" : $"<color=green>Read Note</color>";
    }
}
