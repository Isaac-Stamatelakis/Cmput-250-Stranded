using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rooms;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using PlayerModule;

public class NewLevelInteractableObject : InteractableGameObject, IRoomClearListener
{
    [SerializeField] private Level level;
    public override void Start()
    {
        defaultMaterial = GetComponent<TilemapRenderer>().material;
    }
    public override string getInteractText()
    {
        bool clear = LevelManager.getInstance().CurrentLevel.CurrentRoomClear();
        if (!clear) {
            return "<color=red>Kill all zombies to proceed</color>";
        }
        if (clear && level == null) {
            return "<color=red>No attached level</color>";
        }
        return $"<color=green>Proceed to {level.name}</color>";
    }

    public override void interact()
    {
        LevelManager levelManager = LevelManager.getInstance();
        bool clear = levelManager.CurrentLevel.CurrentRoomClear();
        if (clear) {
            levelManager.playerData = Player.Instance.serialize();
            LevelManager.getInstance().CurrentLevelPrefab = level;
            SceneManager.LoadScene("LevelScene");
        }
    }
    /*
    private IEnumerator loadLevel() {
        if (newLevelReference == null) {
            Debug.LogError($"Level reference for {name} is null");
            yield break;
        }
        var handle = Addressables.LoadAssetAsync<GameObject>(newLevelReference);
        yield return handle;
        if (handle.Status == AsyncOperationStatus.Failed) {
            Debug.LogError($"Could not load level: {handle.OperationException}");
            yield break;
        }
        GameObject result = handle.Result;
        if (result == null) {
            Debug.LogError($"Level was null for {name}");
            yield break;
        }
        Level assetLevel = result.GetComponent<Level>();
        if (assetLevel == null) {
            Debug.LogError($"Level reference for {name} is not a level");
            yield break;
        }
        this.level = assetLevel;
        Addressables.Release(handle);
    }
    */
    public override void highlight()
    {
        GetComponent<TilemapRenderer>().material = highlightShader;
    }

    public override void unhighlight()
    {
        GetComponent<TilemapRenderer>().material = defaultMaterial;
    }

    public void trigger()
    {
        //StartCoroutine(loadLevel());
    }
}
