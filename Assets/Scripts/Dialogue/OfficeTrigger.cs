using System.Collections;
using UnityEngine;
using Rooms;

namespace Dialogue
{
    public class OfficeTrigger : TriggerableEvent
    {
        [SerializeField] private PlayerTutorialManager playerTutorialManagerPrefab;
        [SerializeField] private DialogObject singleDialog;
        public static bool hasPlayedDialogue = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!hasPlayedDialogue && other.CompareTag("Player"))
            {
                trigger();
            }
        }

        public override void trigger()
        {
            if (!hasPlayedDialogue)
            {
                hasPlayedDialogue = true;
                DialogueState.IsDialogueActive = true; // Pause enemies and projectiles

                if (singleDialog != null)
                {
                    DialogUIController.Instance.DisplayDialogue(singleDialog, EndDialogue);
                }
                else
                {
                    Debug.LogWarning("No DialogObject assigned to OfficeTrigger.");
                }
            }
        }

        private void EndDialogue()
        {
            DialogueState.IsDialogueActive = false; // Resume enemy behavior
        }
    }
}
