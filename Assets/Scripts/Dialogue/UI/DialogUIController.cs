using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public delegate void DialogCallBack();

    public class DialogUIController : MonoBehaviour
    {
        [SerializeField] private DialogueBoxUI dialogueBoxUI;
        private static DialogUIController instance;
        public static DialogUIController Instance => instance;
        public bool ShowingDialog => dialogueBoxUI.gameObject.activeInHierarchy;

        internal void DisplayDialogue(object dialogueTree)
        {
            throw new NotImplementedException();
        }

        public void Awake()
        {
            instance = this;
        }

        public void DisplayDialogue(DialogObject dialogObject)
        {
            dialogueBoxUI.gameObject.SetActive(true);
            dialogueBoxUI.DisplayDialogue(dialogObject, OnDialogueComplete);
        }

        public void DisplayDialogue(DialogObject dialogObject, DialogCallBack callBack)
        {
            dialogueBoxUI.gameObject.SetActive(true);
            dialogueBoxUI.DisplayDialogue(dialogObject, callBack);
        }

        private void OnDialogueComplete()
        {
            // Load the ending cutscene if the last dialogue is displayed
            if (dialogueBoxUI.CurrentDialog == "nd_p3")
            {
                NextSceneLoader.Instance.LoadScene("ending cutscene");
            }
        }
    }
}
