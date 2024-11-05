using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBox;
    public float typingSpeed = 0.05f;

    Queue<string> dialogues;

    bool isDialogueActive = false;
    Coroutine typingCoroutine;

    bool isTyping = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogues = new Queue<string>();
        dialogueBox.SetActive(false);
        dialogueText.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
                dialogueText.text = dialogues.Peek();
                dialogues.Dequeue();
                isTyping = false;
            }
            else
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(List<string> newDialogue)
    {
        if (isDialogueActive) return;

        dialogueBox.SetActive(true);
        dialogues.Clear();

        foreach (string sentence in newDialogue)
        {
            dialogues.Enqueue(sentence);
        }
        isDialogueActive = true;
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (dialogues.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = dialogues.Dequeue();

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    public void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialogueBox.SetActive(false);
        isDialogueActive = false;
        dialogueText.text = "";
    }

    IEnumerator TypeSentence(string senetence)
    {

        dialogueText.text = "";
        isTyping = true;
        foreach (char letter in senetence.ToCharArray())
        {
            if (dialogues.Count == 0 && Input.GetKeyDown(KeyCode.E))
            {
                dialogueText.text = senetence;
            }
            else
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        isTyping = false;
    }
}