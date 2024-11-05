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
        private Color baseColor;
        private IndexCallBack hoverCallback;
        private int index;
        public void display(Sprite sprite, int index, IndexCallBack clickCallback, IndexCallBack hoverCallback) {
            this.hoverCallback = hoverCallback;
            baseColor = GetComponent<Outline>().effectColor;
            this.index = index;
            this.image.sprite = sprite;
            button.onClick.AddListener(() => {
                clickCallback(index);
            });
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            GetComponent<Outline>().effectColor = Color.yellow;
            hoverCallback(index);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GetComponent<Outline>().effectColor = baseColor;
            hoverCallback(-1);
        }
    }
}

