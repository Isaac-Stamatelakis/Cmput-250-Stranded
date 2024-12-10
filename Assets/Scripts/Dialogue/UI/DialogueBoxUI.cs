using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayerModule;

namespace Dialogue
{
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
        private bool canFastSkip = true;
        private DialogCallBack callBack;
        private HashSet<DialogObject> previousDialogs = new HashSet<DialogObject>();

        public string CurrentDialog => currentDialog?.name;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                continuePress();
            }
            if (Input.GetKey(KeyCode.Space) && canFastSkip)
            {
                continueHold();
            }
        }

        private void continuePress()
        {
            if (writing)
            {
                return;
            }
            if (currentDialog is DialogSentence dialogSentence)
            {
                DisplayDialogue(dialogSentence.nextDialog);
            }
        }

        private void continueHold()
        {
            if (writing)
            {
                skipChars += 50 * Time.deltaTime;
            }
        }

        // Overload to accept a callback
        public void DisplayDialogue(DialogObject dialogue, DialogCallBack callBack)
        {
            this.callBack = callBack;
            DisplayDialogue(dialogue); // Call the existing method to handle the dialogue display
        }

        private void EndDialogSequence()
        {
            Player.Instance.setDialog(false);
            gameObject.SetActive(false);
            callBack?.Invoke(); // Invoke the callback if set
            callBack = null;
            previousDialogs = new HashSet<DialogObject>();
        }
        public void DisplayDialogue(DialogObject dialogue)
        {
            Player.Instance.setDialog(true);
            if (previousDialogs.Contains(dialogue))
            {
                Debug.LogWarning($"Duplicate dialog would have resulted in a an infinite loop {dialogue.name}");
                EndDialogSequence();
                return;
            }
            skipChars = 0;
            
            if (dialogue == null)
            {
                EndDialogSequence();
                return;
            }
            previousDialogs.Add(dialogue);
            
            /* Commented out by Isaac: Don't think we need this
            // Check if this is the final dialogue ('nd_p3')
            if (dialogue.name == "nd_p3")
            {
                // Trigger ending cutscene
                NextSceneLoader.Instance.LoadScene("ending cutscene");
                return;
            }
            */

            spaceInfoPanel.gameObject.SetActive(true);
            TextMeshProUGUI spaceText = spaceInfoPanel.GetComponentInChildren<TextMeshProUGUI>();
            spaceText.text = "Hold Space to Fast Forward";
            this.currentDialog = dialogue;

            // Clear previous responses
            for (int i = 0; i < responseList.transform.childCount; i++)
            {
                Destroy(responseList.transform.GetChild(i).gameObject);
            }

            this.PortraitImage.sprite = dialogue.Speaker.sprite;
            this.NameText.text = dialogue.Speaker.name;
            StartCoroutine(displayTextCoroutine(dialogue.Text));
        }

        private void displayResponses(DialogueTree dialogueTree)
        {
            currentTree = dialogueTree;
            foreach (var dialogResponse in dialogueTree.responses)
            {
                DialogueResponseUI dialogueResponseUI = Instantiate(responsePrefab);
                dialogueResponseUI.transform.SetParent(responseList.transform, false);
                dialogueResponseUI.display(dialogResponse.Text, dialogueTree.responses.IndexOf(dialogResponse), responseClick);
            }
        }

        private void responseClick(int index)
        {
            DialogObject dialogObject = currentTree.responses[index].dialog;
            DisplayDialogue(dialogObject);
        }

        private IEnumerator displayTextCoroutine(string text)
        {
            writing = true;
            string displayText = "";
            foreach (char c in text)
            {
                displayText += c;
                ContentText.text = displayText;
                if (skipChars > 0)
                {
                    skipChars--;
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    yield return new WaitForSeconds(0.05f);
                }
            }
            ContentText.text = displayText;
            writing = false;

            if (currentDialog is DialogueTree dialogueTree)
            {
                spaceInfoPanel.SetActive(false);
                displayResponses(dialogueTree);
            }
            else
            {
                TextMeshProUGUI spaceText = spaceInfoPanel.GetComponentInChildren<TextMeshProUGUI>();
                spaceText.text = "<color=green>Press Space to Continue</color>";
            }
        }
    }
}
