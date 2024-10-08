using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;
using Rooms;
namespace Dialogue {
    public class DialogueSequencer : TriggerableEvent
    {
        public DialogueTree dialogueTree;
        public override void trigger()
        {
            DialogUIController.Instance.DisplayDialogue(dialogueTree);
        }
    }
}

