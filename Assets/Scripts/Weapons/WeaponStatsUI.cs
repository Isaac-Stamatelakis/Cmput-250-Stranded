using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro; // Import TextMeshPro namespace
using UnityEngine.UI; // Keep this for Image type

public class WeaponStatsUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI attackTimeText;
    [SerializeField] private TextMeshProUGUI knockbackText;
    [SerializeField] private Image artworkImage;

    private static WeaponStatsUI instance;
    public static WeaponStatsUI Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Duplicate WeaponStatsUI detected and destroyed.");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("WeaponStatsUI instance set.");
    }


    private void Start()
    {
        if (weaponNameText == null) Debug.LogError("Weapon Name Text is not assigned.");
        if (damageText == null) Debug.LogError("Damage Text is not assigned.");
        if (rangeText == null) Debug.LogError("Range Text is not assigned.");
        if (attackTimeText == null) Debug.LogError("Attack Time Text is not assigned.");
        if (knockbackText == null) Debug.LogError("Knockback Text is not assigned.");
        if (artworkImage == null) Debug.LogError("Artwork Image is not assigned.");

        if (weaponNameText != null) weaponNameText.text = "";
        if (damageText != null) damageText.text = "";
        if (rangeText != null) rangeText.text = "";
        if (attackTimeText != null) attackTimeText.text = "";
        if (knockbackText != null) knockbackText.text = "";
        if (artworkImage != null)
        {
            artworkImage.sprite = null;
            artworkImage.color = new Color(0, 0, 0, 0);
        }
    }

    public void UpdateWeaponStats(Weapon weapon)
    {
        if (weapon == null) return;

        weaponNameText.text = weapon.weaponName;
        damageText.text = $"Damage: {weapon.damage}";
        rangeText.text = $"Range: {weapon.range:F1}";
        attackTimeText.text = $"Attack Time: {weapon.attackTime:F1}";
        knockbackText.text = $"Knockback: {weapon.knockback:F1}";
        artworkImage.sprite = weapon.artwork;
        artworkImage.color = Color.white;
        Debug.Log("Weapon status updated");
    }
}


