using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PlayerModule {
public class PlayerExperienceUI : MonoBehaviour
{
    [SerializeField] private Scrollbar experienceScrollbar;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private FancyTextUI levelUpText;
    [SerializeField] private Button levelUpButton;
    [SerializeField] private PlayerLevelUpSelectorUI playerLevelUpSelectorUIPrefab;
    [SerializeField] private GridLayoutGroup selectedUpgradeDisplay;
    [SerializeField] List<PlayerUpgradeSpritePair> playerUpgradeSpritePairs;
    private Dictionary<PlayerUpgrade, Sprite> playerUpgradeSpriteDict;
    public void Start() {
        levelUpButton.onClick.AddListener(() => {
            PlayerLevelComponent playerLevelComponent = Player.Instance.GetComponent<PlayerLevelComponent>();
            if (playerLevelComponent.SelectableUpgrades.Count == 0) {
                return;
            }
            PlayerLevelUpSelectorUI instantiated = GameObject.Instantiate(playerLevelUpSelectorUIPrefab);
            instantiated.display(playerLevelComponent.SelectableUpgrades);
            Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();
            instantiated.transform.SetParent(canvas.transform,false);
            hideLevelUpOption();
        });
        levelUpButton.gameObject.SetActive(false);
        playerUpgradeSpriteDict = new Dictionary<PlayerUpgrade, Sprite>();
        foreach (var pair in playerUpgradeSpritePairs) {
            if (playerUpgradeSpriteDict.ContainsKey(pair.PlayerUpgrade)) {
                Debug.LogWarning($"Duplicate sprite pair for {pair.PlayerUpgrade}");
            }
            playerUpgradeSpriteDict[pair.PlayerUpgrade] = pair.Sprite;
        }
    }
    public void displayExperience(int level, float experience, float levelUpExperience) {
        if (experienceText == null || experienceScrollbar == null) {
            Debug.LogWarning("Experience values not set in playerui");
            return;
        }
        experienceText.text = (level+1).ToString();
        experienceScrollbar.size = experience/levelUpExperience;
    }
    public Sprite getUpgradeSprite(PlayerUpgrade playerUpgrade) {
        if (playerUpgradeSpriteDict.ContainsKey(playerUpgrade)) {
            return playerUpgradeSpriteDict[playerUpgrade];
        }
        return null;
    }

    public void displayLevelUpOption() {
        PlayerLevelComponent playerLevelComponent = Player.Instance.GetComponent<PlayerLevelComponent>();
        if (playerLevelComponent.SelectableUpgrades.Count == 0) {
            return;
        }
        levelUpButton.gameObject.SetActive(true);
        Color[] levelUpColors = {
            Color.red,     // Color for Level Up
            Color.yellow,  // Second color
            Color.green,   // Third color
            Color.blue,    // Fourth color
            Color.magenta  // Fifth color
        };
        levelUpText.display("LEVEL UP",levelUpColors);
    }

    public void displayUpgrade(PlayerUpgrade playerUpgrade) {
        Sprite sprite = getUpgradeSprite(playerUpgrade);
        GameObject imageObject = new GameObject();
        imageObject.name = $"{playerUpgrade}_element";
        Image image = imageObject.AddComponent<Image>();
        image.sprite = sprite;
        imageObject.transform.SetParent(selectedUpgradeDisplay.transform,false);

    }

    public void hideLevelUpOption() {
        levelUpText.stop();
        levelUpButton.gameObject.SetActive(false);
    }
}
}

