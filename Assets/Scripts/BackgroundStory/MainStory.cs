using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainStory : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("IsaacLevelScene 1", LoadSceneMode.Single);
    }
}
