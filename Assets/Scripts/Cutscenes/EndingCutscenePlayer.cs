using System.Collections;
using System.Collections.Generic;
using Dialogue;
using Difficulty;
using PlayerModule;
using Rooms;
using UnityEngine;
using SummaryPage;

public class EndingCutscenePlayer : CutScenePlayer
{
    [SerializeField] private PostGameSummaryUI postGameSummary;
    protected override void OnEnd()
    {
        LevelManager levelManager = LevelManager.getInstance();
        PlayerData playerData = levelManager.playerData;
        DifficultyModifier modifier = levelManager.DifficultyModifier;
        int deaths = levelManager.Deaths;
        postGameSummary.DisplaySummary(playerData,modifier, deaths);
        postGameSummary.gameObject.SetActive(true);
    }
}
