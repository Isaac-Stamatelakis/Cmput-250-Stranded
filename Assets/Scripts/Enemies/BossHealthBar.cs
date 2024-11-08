using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;
    public void display(float health, float maxHealth, Sprite sprite, string name) {
        image.sprite = sprite;
        text.text = $"{name}: {health : F0} / {maxHealth : F0}";
        scrollbar.size = health/maxHealth;
    }
}
