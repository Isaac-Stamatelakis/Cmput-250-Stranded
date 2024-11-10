using System.Collections;
using UnityEngine;
using TMPro;

namespace DialogueSystem
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DialogueLine : MonoBehaviour
    {
        [Header("Text Settings")]
        public TextMeshProUGUI textHolder;  // Automatically assigned in Awake
        [TextArea]
        [SerializeField] private string dialogueText = "Hello, this is a test dialogue."; // Example text to test typing
        [SerializeField] private Color textColor = Color.white;
        [SerializeField] private TMP_FontAsset textFont;
        [SerializeField] private float fontSize = 36f; // Adjustable font size

        [Header("Typing Settings")]
        [SerializeField] private float typingDelay = 0.05f; // Delay between each character
        [SerializeField] private float lineDelay = 1f; // Delay after the line is completed

        [Header("Sound Settings")]
        [SerializeField] private AudioClip typingSound;
        private AudioSource audioSource;

        private void Awake()
        {
            // Assign TextMeshProUGUI component if it's not already set
            textHolder = GetComponent<TextMeshProUGUI>();
            if (textHolder == null)
            {
                Debug.LogError("TextMeshProUGUI component is missing. Please assign it.");
                return;
            }

            // Setup TextMeshPro properties
            textHolder.text = ""; // Clear any existing text
            textHolder.color = textColor;
            textHolder.font = textFont;
            textHolder.fontSize = fontSize; // Set font size from Inspector

            // Setup AudioSource component for typing sound
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        private void Start()
        {
            // Start the typing effect with debug to confirm script start
            Debug.Log("Starting typing effect.");
            StartCoroutine(TypeText(dialogueText));
        }

        private IEnumerator TypeText(string input)
        {
            textHolder.text = ""; // Clear text initially

            foreach (char letter in input.ToCharArray())
            {
                textHolder.text += letter; // Add one character at a time
                PlayTypingSound();
                yield return new WaitForSeconds(typingDelay); // Delay between each character
            }

            yield return new WaitForSeconds(lineDelay); // Delay after finishing line
            Debug.Log("Typing effect completed.");
        }

        private void PlayTypingSound()
        {
            if (typingSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(typingSound); // Play typing sound for each character
            }
            else
            {
                Debug.LogWarning("Typing sound or AudioSource is not assigned.");
            }
        }
    }
}
 