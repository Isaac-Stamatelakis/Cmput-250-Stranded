using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rooms;
using PlayerModule;

namespace Dialogue {
    public class TutorialDialogSequencer : TriggerableEvent
    {
        [SerializeField] private PlayerTutorialManager playerTutorialManagerPrefab;
        public DialogueTree dialogueTree;
        private static bool hasPlayedDialogue = false;
        public override void trigger()
        {
            if (!hasPlayedDialogue)
            {
                hasPlayedDialogue = true;
                DialogUIController.Instance.DisplayDialogue(dialogueTree, displayTutorial);
            }
        }

        public static void ResetDialogueFlag()
        {
            hasPlayedDialogue = false;
        }

        private void displayTutorial() {
            PlayerTutorialManager playerTutorialManager = GameObject.Instantiate(playerTutorialManagerPrefab);
            Transform canvasTransform = GameObject.Find("Canvas").transform;
            playerTutorialManager.transform.SetParent(canvasTransform,false);
            playerTutorialManager.transform.SetAsFirstSibling();
        }
    }
}

