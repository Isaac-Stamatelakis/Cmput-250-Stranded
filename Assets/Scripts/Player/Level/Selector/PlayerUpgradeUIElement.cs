using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PlayerModule {
    public class PlayerUpgradeUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private Button button;
        private IndexCallBack hoverCallback;
        private int index;
        public void display(Sprite sprite, int index, IndexCallBack clickCallback, IndexCallBack hoverCallback) {
            this.hoverCallback = hoverCallback;
            this.index = index;
            this.image.sprite = sprite;
            button.onClick.AddListener(() => {
                clickCallback(index);
            });
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            hoverCallback(index);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hoverCallback(-1);
        }
    }
}

