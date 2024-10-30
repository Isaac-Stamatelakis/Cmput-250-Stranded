using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TitleScreen {
    public class PlayButton : MonoBehaviour
    {
        private static readonly string SCENE_NAME = "IsaacLevelScene";
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(loadScene);
        }

        private void loadScene() {
            SceneManager.LoadScene(SCENE_NAME);
        }
    }
}

