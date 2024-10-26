using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PlayerModule {
    public class PlayerLevelUpSelectorUI : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI upgradeDescription;
        [SerializeField] private GridLayoutGroup upgradeList;
        [SerializeField] PlayerUpgradeUIElement uiElementPrefab;
        private List<PlayerUpgrade> playerUpgrades;
        public void display(List<PlayerUpgrade> playerUpgrades) {
            backButton.onClick.AddListener(() => {
                GameObject.Destroy(gameObject);
            });
            PlayerExperienceUI playerExperienceUI = Player.Instance.PlayerUI.PlayerExperienceUI;
            this.playerUpgrades = playerUpgrades;
            for (int i = 0; i < playerUpgrades.Count; i++) {
                PlayerUpgrade playerUpgrade = playerUpgrades[i];
                Sprite sprite = playerExperienceUI.getUpgradeSprite(playerUpgrade);
                PlayerUpgradeUIElement uIElement = GameObject.Instantiate(uiElementPrefab);
                uIElement.display(sprite,i,upgradeSelect,displayDescription);
                uIElement.transform.SetParent(upgradeList.transform);
            }
        }

        public void upgradeSelect(int index) {
            GameObject.Destroy(gameObject);
            PlayerLevelComponent playerLevelComponent = Player.Instance.GetComponent<PlayerLevelComponent>();
            playerLevelComponent.addComponent(playerUpgrades[index]);
            playerLevelComponent.iterateUpgrades();
            PlayerExperienceUI playerExperienceUI = Player.Instance.PlayerUI.PlayerExperienceUI;
            playerExperienceUI.hideLevelUpOption();
            playerExperienceUI.displayUpgrade(playerUpgrades[index]);
        }

        public void displayDescription(int index) {
            bool hide = index <= -1;
            if (hide) {
                upgradeDescription.text = "";
                return;
            }
            upgradeDescription.text = playerUpgrades[index].getDescription();
        }
    }
    public delegate void IndexCallBack(int index);
    [System.Serializable]
    public class PlayerUpgradeSpritePair {
        public PlayerUpgrade PlayerUpgrade;
        public Sprite Sprite;
    }
}

