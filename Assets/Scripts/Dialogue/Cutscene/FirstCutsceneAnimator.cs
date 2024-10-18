using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
    public class FirstCutsceneAnimator : CutsceneAnimator
    {
        [SerializeField] private PlayerTutorialManager playerTutorialManagerPrefab;
        protected override void finishActions()
        {
            PlayerTutorialManager playerTutorialManager = GameObject.Instantiate(playerTutorialManagerPrefab);
            Transform canvasTransform = GameObject.Find("Canvas").transform;
            playerTutorialManager.transform.SetParent(canvasTransform,false);
            playerTutorialManager.transform.SetAsFirstSibling();
        }
    }

}
