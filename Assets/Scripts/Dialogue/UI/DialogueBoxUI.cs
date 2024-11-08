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
        public GridLayoutGroup responseList;
        public DialogueResponseUI responsePrefab;
        public GameObject spaceInfoPanel;
        private bool writing;
        private DialogueTree currentTree;
        private DialogObject currentDialog;
        private float skipChars = 0;
        private Color baseButtonColor;
        private bool canFastSkip = true;

        public void Update() {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                countinuePress();
            }
            if (Input.GetKey(KeyCode.Space) && canFastSkip)
            {
                countineHold();
            }


            if (Input.GetKeyUp(KeyCode.Space))
            {
                //canFastSkip = true;
            }
        }

        private void countinuePress() {
            if (writing) {
                return;
            }
            if (currentDialog is DialogSentence dialogSentence) {
                //canFastSkip = false;
                DisplayDialogue(dialogSentence.nextDialog);
            }
        }

        private void countineHold() {
            if (writing) {
                skipChars += 50*Time.deltaTime;
            }
        }
        public void DisplayDialogue(DialogObject dialogue) {
            skipChars = 0;
            Player.Instance.setDialog(dialogue!=null);
            if (dialogue == null) {
                gameObject.SetActive(false);
                return;
            }
            spaceInfoPanel.gameObject.SetActive(true);
            TextMeshProUGUI spaceText = spaceInfoPanel.GetComponentInChildren<TextMeshProUGUI>();
            spaceText.text = "Hold Space to Fast Forward";
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
                ContentText.text = $"{displayText}";
                if (skipChars > 0) {
                    skipChars--;
                    yield return new WaitForEndOfFrame();
                } else {
                    yield return new WaitForSeconds(0.05f);
                }
            }
            ContentText.text = displayText;
            this.writing = false;
            
            if (currentDialog is DialogueTree dialogueTree) {
                spaceInfoPanel.gameObject.SetActive(false);
                displayResponses(dialogueTree);
            } else {
                TextMeshProUGUI spaceText = spaceInfoPanel.GetComponentInChildren<TextMeshProUGUI>();
                spaceText.text = "<color=green>Press Space to Continue</color>";
            }
        }
    }
}

