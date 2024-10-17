using UnityEngine;
using Dialogue;  // Import the Dialogue namespace to access DialogUIController

public class DialogueTutorialController : MonoBehaviour
{
    public GameObject dialogueTutorial;  // The DialogueTutorial GameObject or Canvas
    private bool tutorialVisible = false;
    private bool tutorialCompleted = false;  // Track if the tutorial has already been completed

    void Start()
    {
        // Initially hide the tutorial dialogue
        if (dialogueTutorial != null)
            dialogueTutorial.SetActive(false);
    }

    void Update()
    {
        // Ensure that the tutorial only starts after the DialogBox is not active, and it's not already completed
        if (!tutorialCompleted && DialogUIController.Instance != null && !DialogUIController.Instance.ShowingDialog && !tutorialVisible)
        {
            // DialogBox is inactive, the tutorial hasn't been shown yet, so show the tutorial
            tutorialVisible = true;
            if (dialogueTutorial != null)
                dialogueTutorial.SetActive(true);
        }

        // If the DialogBox becomes active and the tutorial is visible, hide the tutorial
        if (!tutorialCompleted && DialogUIController.Instance != null && DialogUIController.Instance.ShowingDialog && tutorialVisible)
        {
            tutorialVisible = false;
            if (dialogueTutorial != null)
                dialogueTutorial.SetActive(false);
        }
    }

    // Method to be called when the third instruction is completed
    public void CompleteTutorial()
    {
        // This will prevent the tutorial from being shown again
        tutorialCompleted = true;
        tutorialVisible = false;

        // Hide the tutorial
        if (dialogueTutorial != null)
        {
            dialogueTutorial.SetActive(false);
        }
    }
}
