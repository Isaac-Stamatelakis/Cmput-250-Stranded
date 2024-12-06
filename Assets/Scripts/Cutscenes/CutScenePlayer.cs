using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StoryElement
{
    [SerializeField, TextArea(5, 10)] public string Text;
    public Sprite Image;
    public AudioClip Sound;
    public bool DisableImage = false;
}
public abstract class CutScenePlayer : MonoBehaviour
{
    public StoryElement[] StoryElements;

    private TextMeshProUGUI textElement;

    private AudioSource audioSource;

    public Image ImageElement;

    private int currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        textElement = GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
        DisplayStoryElement(StoryElements[0]);
    }

    private void DisplayStoryElement(StoryElement storyElement)
    {
        textElement.text = storyElement.Text;
        ImageElement.sprite = storyElement.Image;
        audioSource.clip = storyElement.Sound;
        
        if (storyElement.DisableImage)
        {
            ImageElement.gameObject.SetActive(false);
        } else if (ImageElement.sprite != null)
        {
            ImageElement.gameObject.SetActive(true);
            ImageElement.sprite = storyElement.Image;
        }
        
    }

    protected abstract void OnEnd();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentIndex++;
            if (currentIndex >= StoryElements.Length)
            {
                OnEnd();
                return;
            }
            DisplayStoryElement(StoryElements[currentIndex]);
        }
    }
}
