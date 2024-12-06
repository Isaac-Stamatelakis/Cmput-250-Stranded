using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningCutScenePlayer : CutScenePlayer
{
    protected override void OnEnd()
    {
        SceneManager.LoadScene("LevelScene");
    }
}
