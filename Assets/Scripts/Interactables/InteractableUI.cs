using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InteractableUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private TextMeshProUGUI keyText;
    public void display(string key, string text) {
        gameObject.SetActive(true);
        contentText.text = text;
        keyText.text = key;
    }
    public void hide() {
        gameObject.SetActive(false);
    }
}
