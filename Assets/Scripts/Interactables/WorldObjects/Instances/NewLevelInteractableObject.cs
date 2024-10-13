using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rooms;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class NewLevelInteractableObject : InteractableGameObject
{
    [SerializeField] private Level newLevel;
    public override void Start()
    {
        defaultMaterial = GetComponent<TilemapRenderer>().material;
    }
    public override string getInteractText()
    {
        bool clear = LevelManager.getInstance().CurrentLevel.CurrentRoomClear();
        if (clear) {
            return newLevel != null ? $"Proceed to {newLevel.name}" : "";
        } else {
            return "<color=red>Kill all zombies to proceed</color>";
        }
    }

    public override void interact()
    {
        bool clear = LevelManager.getInstance().CurrentLevel.CurrentRoomClear();
        if (clear) {
            LevelManager.getInstance().CurrentLevelPrefab = newLevel;
            SceneManager.LoadScene("LevelScene");
        }
        
    }
    public override void highlight()
    {
        GetComponent<TilemapRenderer>().material = highlightShader;
    }

    public override void unhighlight()
    {
        GetComponent<TilemapRenderer>().material = defaultMaterial;
    }
}
