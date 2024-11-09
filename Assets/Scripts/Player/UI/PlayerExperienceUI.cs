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
    [SerializeField] private TextMeshProUGUI experienceTextRatio;
    [SerializeField] private Button levelUpButton;
    [SerializeField] private PlayerLevelUpSelectorUI playerLevelUpSelectorUIPrefab;
    [SerializeField] private GridLayoutGroup selectedUpgradeDisplay;
    [SerializeField] List<PlayerUpgradeSpritePair> playerUpgradeSpritePairs;
    private Dictionary<PlayerUpgrade, Sprite> playerUpgradeSpriteDict;
    
    private bool selectorDisplayed;
    public bool SelectorDisplayed => selectorDisplayed;
    public void Start() {
        levelUpButton.onClick.AddListener(() => {
            displayLevelSelector(null);
        });
        levelUpButton.gameObject.SetActive(false);
        createDict();
        
    }

    public void createDict() {
        playerUpgradeSpriteDict = new Dictionary<PlayerUpgrade, Sprite>();
        foreach (var pair in playerUpgradeSpritePairs) {
            if (playerUpgradeSpriteDict.ContainsKey(pair.PlayerUpgrade)) {
                Debug.LogWarning($"Duplicate sprite pair for {pair.PlayerUpgrade}");
            }
            playerUpgradeSpriteDict[pair.PlayerUpgrade] = pair.Sprite;
        }
    }

    public void displayLevelSelector(VoidCallBack callBack) {
        if (selectorDisplayed) {
            return;
        }
        selectorDisplayed = true;
        PlayerLevelComponent playerLevelComponent = Player.Instance.GetComponent<PlayerLevelComponent>();
        playerLevelComponent.generateSelectableUpgrades();
        if (playerLevelComponent.SelectableUpgrades.Count == 0) {
            return;
        }
        PlayerLevelUpSelectorUI instantiated = GameObject.Instantiate(playerLevelUpSelectorUIPrefab);
        instantiated.display(playerLevelComponent.SelectableUpgrades,callBack);
        Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();
        instantiated.transform.SetParent(canvas.transform,false);
        hideLevelUpOption();
    }

    public void setSelectorDisplayed(bool state) {
        selectorDisplayed = state;
    }
    public void displayExperience(int level, float experience, float levelUpExperience) {
        if (experienceText == null || experienceScrollbar == null) {
            Debug.LogWarning("Experience values not set in playerui");
            return;
        }
        experienceText.text = (level+1).ToString();
        experienceScrollbar.size = experience/levelUpExperience;
        experienceTextRatio.text = $"{experience} / {levelUpExperience}";
    }
    public Sprite getUpgradeSprite(PlayerUpgrade playerUpgrade) {
        if (playerUpgradeSpriteDict == null) {
            createDict();
        }
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

