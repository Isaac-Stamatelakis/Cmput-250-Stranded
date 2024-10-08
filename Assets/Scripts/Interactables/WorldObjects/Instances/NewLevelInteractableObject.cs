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
        return "Proceed to the Next Level";
        return $"Proceed to {newLevel.name}";
    }

    public override void interact()
    {
        LevelManager.getInstance().CurrentLevelPrefab = newLevel;
        SceneManager.LoadScene("LevelScene");
    }
    public override void highlight()
    {
        GetComponent<TilemapRenderer>().material = highlightShader;
    }

    public override void unhighlight()
    {
        GetComponent<TilemapRenderer>().material = defaultMaterial;
    }
    public override bool isInteractable()
    {
        return LevelManager.getInstance().CurrentLevel.CurrentRoomClear();
    }
}
