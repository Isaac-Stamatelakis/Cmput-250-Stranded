using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] GameObject interactionPrompt;

    bool canInteract = false;
    NPCDialogue currentNPC;

    void Start()
    {
        interactionPrompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            if (currentNPC != null)
            {
                currentNPC.TriggerDialogue();
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            interactionPrompt.SetActive(true);
            canInteract = true;
            currentNPC = other.GetComponent<NPCDialogue>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            interactionPrompt.SetActive(false);
            canInteract = false;
            currentNPC.EndDialogue();
            currentNPC = null;
        }
    }
}