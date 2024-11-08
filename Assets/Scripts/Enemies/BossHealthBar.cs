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
    private static BossHealthBar instance;
    public static BossHealthBar Instance => instance;
    public void Awake() {
        instance = this;
        gameObject.SetActive(false);
    }
    public void display(float health, float maxHealth, Sprite sprite, string name) {
        image.sprite = sprite;
        text.text = $"{name}: {health:F0} / {maxHealth:F0}";
        scrollbar.size = health/maxHealth;
    }
}
