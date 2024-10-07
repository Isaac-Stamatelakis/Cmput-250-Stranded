using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
    public class DialogUIController : MonoBehaviour
    {
        [SerializeField] private DialogueBoxUI dialogueBoxUI;
        private static DialogUIController instance;
        public static DialogUIController Instance => instance;
        public bool ShowingDialog => dialogueBoxUI.gameObject.activeInHierarchy;
        public void Awake() {
            instance = this;
        }
        public void DisplayDialogue(DialogObject dialogObject) {
            dialogueBoxUI.gameObject.SetActive(true);
            dialogueBoxUI.DisplayDialogue(dialogObject);
        }
    }
}

