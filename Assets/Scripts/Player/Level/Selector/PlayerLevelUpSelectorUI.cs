using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PlayerModule {
    public delegate void VoidCallBack();
    public class PlayerLevelUpSelectorUI : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI upgradeDescription;
        [SerializeField] private GridLayoutGroup upgradeList;
        [SerializeField] PlayerUpgradeUIElement uiElementPrefab;
        private List<PlayerUpgrade> playerUpgrades;
        private VoidCallBack callBack;
        public void display(List<PlayerUpgrade> playerUpgrades) {
            PlayerExperienceUI playerExperienceUI = Player.Instance.PlayerUI.PlayerExperienceUI;
            Player.Instance.setDialog(true);
            backButton.onClick.AddListener(() => {
                playerExperienceUI.displayLevelUpOption();
                playerExperienceUI.setSelectorDisplayed(false);
                GameObject.Destroy(gameObject);
            });
            
            this.playerUpgrades = playerUpgrades;
            for (int i = 0; i < playerUpgrades.Count; i++) {
                PlayerUpgrade playerUpgrade = playerUpgrades[i];
                Sprite sprite = playerExperienceUI.getUpgradeSprite(playerUpgrade);
                PlayerUpgradeUIElement uIElement = GameObject.Instantiate(uiElementPrefab);
                uIElement.display(sprite,i,upgradeSelect,displayDescription);
                uIElement.transform.SetParent(upgradeList.transform);
            }
        }

        public void display(List<PlayerUpgrade> playerUpgrades, VoidCallBack callBack) {
            this.display(playerUpgrades);
            this.callBack = callBack;
        }

        public void upgradeSelect(int index) {
            GameObject.Destroy(gameObject);
            PlayerLevelComponent playerLevelComponent = Player.Instance.GetComponent<PlayerLevelComponent>();
            playerLevelComponent.AddPlayerUpgrade(playerUpgrades[index]);
            playerLevelComponent.IterateUpgrades();
            PlayerExperienceUI playerExperienceUI = Player.Instance.PlayerUI.PlayerExperienceUI;
            if (playerLevelComponent.RemainingUpgrades <= 0) {
                playerExperienceUI.hideLevelUpOption();
                if (callBack != null) {
                    callBack();
                }
            } else {
                playerExperienceUI.setSelectorDisplayed(false);
                playerExperienceUI.displayLevelSelector(callBack);
            }
        }

        public void displayDescription(int index) {
            bool hide = index <= -1;
            if (hide)
            {
                upgradeDescription.text = "";
                return;
            }
            upgradeDescription.text = playerUpgrades[index].GetDescription();
            
        }

        public void OnDestroy() {
            PlayerExperienceUI playerExperienceUI = Player.Instance.PlayerUI.PlayerExperienceUI;
            playerExperienceUI.setSelectorDisplayed(false);
            Player.Instance.setDialog(false);
        }
    }
    public delegate void IndexCallBack(int index);
    [System.Serializable]
    public class PlayerUpgradeSpritePair {
        public PlayerUpgrade PlayerUpgrade;
        public Sprite Sprite;
    }
}

