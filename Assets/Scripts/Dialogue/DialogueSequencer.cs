using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;
using Rooms;

namespace Dialogue
{
    public class DialogueSequencer : TriggerableEvent
    {
        public DialogueTree dialogueTree;
        private static bool hasPlayedDialogue = false;

        public override void trigger()
        {
            if (!hasPlayedDialogue)
            {
                DialogUIController.Instance.DisplayDialogue(dialogueTree);
                hasPlayedDialogue = true; 
            }
        }

        public static void ResetDialogueFlag()
        {
            hasPlayedDialogue = false; 
        }
    }
}
