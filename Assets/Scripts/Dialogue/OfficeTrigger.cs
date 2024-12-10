
using System.Collections;
using UnityEngine;
using Rooms;
using PlayerModule;

namespace Dialogue
{
    public class OfficeTrigger : TriggerableEvent
    {
        [SerializeField] private PlayerTutorialManager playerTutorialManagerPrefab;
        [SerializeField] private DialogObject introDialog; // For the first room
        [SerializeField] private DialogObject bossRoomDialog; // For the boss room
        [SerializeField] private bool isBossRoom; // Toggle for boss room
        private static bool hasPlayedIntroDialogue = false;
        public static bool hasPlayedBossDialogue = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (isBossRoom && !hasPlayedBossDialogue)
                {
                    TriggerBossDialogue();
                }
                else if (!isBossRoom && !hasPlayedIntroDialogue)
                {
                    TriggerIntroDialogue();
                }
            }
        }

        private void TriggerIntroDialogue()
        {
            hasPlayedIntroDialogue = true;
            PlayDialogue(introDialog);
        }

        private void TriggerBossDialogue()
        {
            hasPlayedBossDialogue = true;
            PlayDialogue(bossRoomDialog);
        }

        private void PlayDialogue(DialogObject dialog)
        {
            if (dialog != null)
            {
                DialogueState.IsDialogueActive = true; // Pause enemies and projectiles
                Player.Instance.setDialog(true);

                DialogUIController.Instance.DisplayDialogue(dialog, EndDialogue);
            }
            else
            {
                Debug.LogWarning("DialogObject is not assigned to this trigger.");
            }
        }

        private void EndDialogue()
        {
            Player.Instance.setDialog(false);
            DialogueState.IsDialogueActive = false; // Resume enemy behavior
        }

        public override void trigger()
        {
            throw new System.NotImplementedException();
        }
    }
}
