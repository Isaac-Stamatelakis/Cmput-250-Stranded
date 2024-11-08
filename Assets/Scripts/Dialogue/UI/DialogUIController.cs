using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class DialogUIController : MonoBehaviour
    {
        [SerializeField] private DialogueBoxUI dialogueBoxUI;
        private static DialogUIController instance;
        public static DialogUIController Instance => instance;
        public bool ShowingDialog => dialogueBoxUI.gameObject.activeInHierarchy;

        // Add fields for the last dialogue name and the ending scene name
        [SerializeField] private string lastDialogName = "nd_p3"; 
        [SerializeField] private string endingSceneName = "ending cutscene"; 

        public void Awake()
        {
            instance = this;
        }

        public void DisplayDialogue(DialogObject dialogObject)
        {
            dialogueBoxUI.gameObject.SetActive(true);
            dialogueBoxUI.DisplayDialogue(dialogObject);

            // Check if the current dialogue is the last one
            if (dialogObject != null && dialogObject.name == lastDialogName)
            {
                PlayEndingCutscene();
            }
        }

        private void PlayEndingCutscene()
        {
            Debug.Log("Playing ending cutscene...");
            NextSceneLoader.Instance.LoadScene(endingSceneName);
        }
    }
}
