using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;
using Rooms;
namespace Dialogue {
    public class DialogueSequencer : TriggerableEvent
    {
        public DialogueTree dialogueTree;
        public DialogueBoxUI dialogueBoxUI;

        public override void trigger()
        {
            dialogueBoxUI.gameObject.SetActive(true);
            dialogueBoxUI.DisplayDialogue(dialogueTree);
        }
    }
}

