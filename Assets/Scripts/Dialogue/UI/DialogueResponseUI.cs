using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Dialogue {
    public class DialogueResponseUI : MonoBehaviour
    {
        public TextMeshProUGUI textElement;
        public Button button;
        private IndexCallback callback;
        private int index;
        public void Start() {
            button.onClick.AddListener(() => {
                callback(index);
            });
        }
        public void display(string text, int index, IndexCallback callback) {
            textElement.text = text;
            this.callback = callback;
            this.index = index;
        }
    }
}

