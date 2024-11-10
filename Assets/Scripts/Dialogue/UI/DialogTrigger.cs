using UnityEngine;

namespace Dialogue
{
    public class DialogTrigger : MonoBehaviour
    {
        public DialogueTree dialogueTree;

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Check if the object that entered is the player
            if (other.CompareTag("Player"))
            {
                // Trigger the dialogue
                DialogUIController.Instance.DisplayDialogue(dialogueTree);

                // Optionally, disable the trigger so it only plays once
                gameObject.SetActive(false);
            }
        }
    }
}
