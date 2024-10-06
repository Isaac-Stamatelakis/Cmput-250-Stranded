using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InteractableUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textElement;
    private string displayText;
    public void Start() {
        displayText = textElement.text;
    }
    public void display(string key, string text) {
        gameObject.SetActive(true);
        textElement.text = displayText.Replace("[KEY]",key).Replace("[TEXT]",text);
    }
    public void hide() {
        gameObject.SetActive(false);
    }
}
