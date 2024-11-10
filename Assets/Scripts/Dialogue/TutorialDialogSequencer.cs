using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
    public class TutorialDialogSequencer : DialogueSequencer
    {
        [SerializeField] private PlayerTutorialManager playerTutorialManagerPrefab;
        public override void trigger()
        {
            DialogUIController.Instance.DisplayDialogue(dialogueTree,displayTutorial);
        }

        private void displayTutorial() {
            PlayerTutorialManager playerTutorialManager = GameObject.Instantiate(playerTutorialManagerPrefab);
            Transform canvasTransform = GameObject.Find("Canvas").transform;
            playerTutorialManager.transform.SetParent(canvasTransform,false);
            playerTutorialManager.transform.SetAsFirstSibling();
        }
    }
}

