using System.Collections;
using System.Collections.Generic;
using Difficulty;
using PlayerModule;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.UI;
using Rooms;
using SummaryPage;

public class EndCutsceneListener : MonoBehaviour
{
    private PlayableDirector director;
    [SerializeField] private Button button;

    [SerializeField] private PostGameSummaryUI postGameSummaryUIPrefab;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(() => {
            LevelManager.getInstance().reset();
            SceneManager.LoadScene("TitleScene");
        });
        director = GetComponent<PlayableDirector>();
        director.stopped += onFinish;
    }

    public void onFinish(PlayableDirector playableDirector) {
        //button.gameObject.SetActive(true);
        PostGameSummaryUI postGameSummaryUI = GameObject.Instantiate(postGameSummaryUIPrefab, transform, false);
        LevelManager levelManager = LevelManager.getInstance();
        DifficultyModifier modifier = levelManager.DifficultyModifier;
        int deaths = levelManager.Deaths;
        PlayerData playerData = levelManager.playerData;

        /*
        DifficultyModifier modifier = DifficultyPresets.Impossible.GetDifficultyModifier();
        int deaths = 3;
        PlayerStats playerStats = new PlayerStats();
        playerStats.Time = 51f;
        playerStats.Kills = 512;
        playerStats.DamageDealt = 143.4123f;
        playerStats.DamageTaken = 143.4123f;
        playerStats.Healing = 143.4123f;
        PlayerData playerData = new PlayerData(0, 0, 0, null, null, playerStats);
        */
        
        
        postGameSummaryUI.DisplaySummary(playerData, modifier, deaths);
    }

    public void Update() {
        if (Input.GetKey(KeyCode.Space)) {
            director.time += 5*Time.deltaTime;
        }
    }
}
