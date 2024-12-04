using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TornPaperInteractable : InteractableGameObject
{
    [SerializeField] private GameObject paperCanvas; 
    private bool allZombiesDefeated = false;

    public override string getInteractText()
    {
        return allZombiesDefeated ? $"Press E to Read Research Log" : string.Empty;
    }

    public override void interact()
    {
        if (!allZombiesDefeated) return;

        Debug.Log("Displaying Research Log...");

        if (paperCanvas != null)
        {
            paperCanvas.SetActive(true); // Show the canvas

            var textComponent = paperCanvas.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void ActivateWhenAllZombiesDefeated()
    {
        Debug.Log("All zombies defeated! Torn paper is now interactable.");
        allZombiesDefeated = true;

        InteractableUIController.Instance.display(
            InteractableDisplayType.Interactable,
            getInteractText(),
            transform.position
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && allZombiesDefeated)
        {
            Debug.Log("Player entered interaction zone for torn paper.");
            InteractableUIController.Instance.display(
                InteractableDisplayType.Interactable,
                getInteractText(),
                transform.position
            );
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited interaction zone for torn paper.");
            InteractableUIController.Instance.hide();

            if (paperCanvas != null)
            {
                paperCanvas.SetActive(false); 
            }
        }
    }
}
