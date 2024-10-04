using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayerModule;

namespace Dialogue {
    public delegate void IndexCallback(int index);
    public class DialogueBoxUI : MonoBehaviour
    {
        public Image PortraitImage;
        public TextMeshProUGUI NameText;
        public TextMeshProUGUI ContentText;
        public Button countinueButton;
        public GridLayoutGroup responseList;
        public DialogueResponseUI responsePrefab;
        private bool writing;
        private DialogueTree currentTree;
        private DialogObject currentDialog;
        private int skipChars = 0;
        public void Start() {
            countinueButton.onClick.AddListener(() => {
                if (writing) {
                    skipChars += 10;
                } else {
                    if (currentDialog is DialogSentence dialogSentence) {
                        DisplayDialogue(dialogSentence.nextDialog);
                    }
                }
            });
        }
        public void DisplayDialogue(DialogObject dialogue) {
            skipChars = 0;
            Player.Instance.setDialog(dialogue!=null);
            if (dialogue == null) {
                gameObject.SetActive(false);
                return;
            }
            this.currentDialog = dialogue;
            for (int i = 0; i < responseList.transform.childCount; i++) {
                GameObject.Destroy(responseList.transform.GetChild(i).gameObject);
            }
            this.PortraitImage.sprite = dialogue.Speaker.sprite;
            this.NameText.text = dialogue.Speaker.name;
            StartCoroutine(displayTextCoroutine(dialogue.Text));
        }

        private void displayResponses(DialogueTree dialogueTree) {
            currentTree = dialogueTree;
            for (int i = 0; i < dialogueTree.responses.Count; i++) {
                DialogueResponseUI dialogueResponseUI = GameObject.Instantiate(responsePrefab);
                dialogueResponseUI.transform.SetParent(responseList.transform,false);
                DialogResponse dialogResponse = dialogueTree.responses[i];
                dialogueResponseUI.display(dialogResponse.Text,i, responseClick);
            }
        }

        private void responseClick(int index) {
            DialogObject dialogObject = currentTree.responses[index].dialog;
            DisplayDialogue(dialogObject);
        }

        private IEnumerator displayTextCoroutine(string text) {
            this.writing = true;
            string displayText = "";
            foreach (char c in text) {
                displayText += c;
                ContentText.text = $"{displayText} ...";
                if (skipChars > 0) {
                    skipChars--;
                    yield return new WaitForEndOfFrame();
                } else {
                    yield return new WaitForSeconds(0.1f);
                }
            }
            ContentText.text = displayText;
            this.writing = false;
            if (currentDialog is DialogueTree dialogueTree) {
                displayResponses(dialogueTree);
            }

        }
    }
}

