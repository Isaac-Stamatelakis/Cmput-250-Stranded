using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void DialogCallBack();
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

        public void DisplayDialogue(DialogObject dialogObject, DialogCallBack callBack) {
            dialogueBoxUI.gameObject.SetActive(true);
            dialogueBoxUI.DisplayDialog(dialogObject,callBack);
        }
    }
}

