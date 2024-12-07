using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayerModule;
using Rooms;
using Dialogue;
using Difficulty;

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
                LevelManager levelManager = LevelManager.getInstance();
                DifficultyModifier difficultyModifier = levelManager.DifficultyModifier;
                if (!difficultyModifier.CheckPoints)
                {
                    levelManager.reset();
                }
                SceneManager.LoadScene(currentSceneIndex);
            });
            Home.onClick.AddListener(() => {
                PauseScreenVerifier pauseScreenVerifier = GameObject.Instantiate(pauseScreenVerifierPrefab, transform, false);
                pauseScreenVerifier.display(titleScreenCallback, "Are you sure you want to exit?", null, "Exit");
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

