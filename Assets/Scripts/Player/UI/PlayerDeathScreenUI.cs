using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayerModule;
using Rooms;
using Dialogue;

namespace PlayerModule {
    public class PlayerDeathScreenUI : MonoBehaviour
    {
        [SerializeField] private Button TryAgain;
        [SerializeField] private Button Home;
        [SerializeField] private PauseScreenVerifier pauseScreenVerifierPrefab;
  
        public void Start() {
            Player.Instance.setDialog(true);
            Time.timeScale = 0;
            TryAgain.onClick.AddListener(() => {
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(currentSceneIndex);
            });
            Home.onClick.AddListener(() => {
                PauseScreenVerifier pauseScreenVerifier = GameObject.Instantiate(pauseScreenVerifierPrefab);
                pauseScreenVerifier.display(titleScreenCallback, "Are you sure you want to exit?", null, "Exit");
                pauseScreenVerifier.transform.SetParent(transform, false);

            });

        }
        private void titleScreenCallback()
        {
            LevelManager.getInstance().reset();
            SceneManager.LoadScene("TitleScene");
            TutorialDialogSequencer.ResetDialogueFlag();
        }

        public void OnDestroy()
        {
            Player.Instance.setDialog(false);
            Time.timeScale = 1;
        }
    }
}

