using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FancyTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textPrefab;
    private Color[] colors;
    private int counter;
    private bool runAnimation;
    private string previouslyDisplayed;
    public void display(string text, Color[] colors) {
        this.colors = colors;
        if (text != previouslyDisplayed) {
            for (int i = 0 ; i < transform.childCount; i++) {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
        }
        previouslyDisplayed = text;
        foreach (char c in text) {
            TextMeshProUGUI textElement = GameObject.Instantiate(textPrefab);
            textElement.text = c.ToString();
            textElement.transform.SetParent(transform,false);
        }
        
        StartCoroutine(startAnimation());
    }

    public void stop() {
        runAnimation = false;
        for (int i = 0 ; i < transform.childCount; i++) {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }

    private IEnumerator startAnimation() {
        runAnimation = true;
        while (runAnimation) {
            counter ++;
            for (int i = 0; i < transform.childCount; i++) {
                TextMeshProUGUI textElement = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
                if (colors != null) {
                    int colorIndex = (i+counter) % colors.Length;
                    textElement.color = colors[colorIndex];
                }
                int alignment = (i+counter) % 4;
                if (alignment % 2 == 0) {
                    textElement.verticalAlignment = VerticalAlignmentOptions.Middle;
                } else if (alignment == 0) {
                    textElement.verticalAlignment = VerticalAlignmentOptions.Top;
                } else if (alignment == 3) {
                    textElement.verticalAlignment = VerticalAlignmentOptions.Bottom;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        
    }

}
