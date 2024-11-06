using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayerModule;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private Button resume;
    [SerializeField] private Button restart;
    [SerializeField] private Button titleScreen;
    [SerializeField] private PauseScreenVerifier pauseScreenVerifierPrefab;

    public void Start() {
        Player.Instance.setDialog(true);
        Time.timeScale = 0;
        resume.onClick.AddListener(() => {
            
            GameObject.Destroy(gameObject);
        });

        restart.onClick.AddListener(() => {
            PauseScreenVerifier pauseScreenVerifier = GameObject.Instantiate(pauseScreenVerifierPrefab);
            pauseScreenVerifier.display(restartCallback,"Are you sure you want to restart?", null,"Restart");
            pauseScreenVerifier.transform.SetParent(transform,false);
        });

        titleScreen.onClick.AddListener(() => {
            PauseScreenVerifier pauseScreenVerifier = GameObject.Instantiate(pauseScreenVerifierPrefab);
            pauseScreenVerifier.display(titleScreenCallback,"Are you sure you want to exit?",null,"Exit");
            pauseScreenVerifier.transform.SetParent(transform,false);
            
        });
    }

    private void restartCallback() {
        SceneManager.LoadScene("LevelScene");
    }

    private void titleScreenCallback() {
        SceneManager.LoadScene("TitleScene");
    }

    public void OnDestroy() {
        Player.Instance.setDialog(false);
        Time.timeScale = 1;
    }
}
