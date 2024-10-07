using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Priority is defined by order, eg Interactables have higher priority than TalkingToDate so if an 
// interactable is near the player and the date is also near, the interactable message will be shown
public enum InteractableDisplayType {
    Interactable,
    TalkToDate
}
public static class InteractableDisplayTypeExtension {
    public static string getKey(this InteractableDisplayType interactableDisplayType) {
        switch (interactableDisplayType) {
            case InteractableDisplayType.Interactable:
                return "E";
            case InteractableDisplayType.TalkToDate:
                return "E";
        }
        throw new System.Exception($"Did not cover case for {interactableDisplayType}");
    }
}
public class InteractableUIController : MonoBehaviour
{
    [SerializeField] private InteractableUI interactableUI;
    private static InteractableUIController instance;
    public static InteractableUIController Instance => instance;
    private static readonly int VERTICAL_PADDING = 150;
    private InteractableDisplayType? displayedInteractable;
    public void Awake() {
        instance = this;
    }

    public void display(InteractableDisplayType interactableDisplayType, string text, Vector3 worldPosition) {
        bool displaying = displayedInteractable != null;
        /*
        if (displaying) {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            interactableUI.transform.position = screenPosition + new Vector3(0,VERTICAL_PADDING,0);
            bool lessPriorityOrEqual = interactableDisplayType <= displayedInteractable;
            if (lessPriorityOrEqual) {
                return;
            }
        }
        */
        displayedInteractable = interactableDisplayType;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        interactableUI.transform.position = screenPosition + new Vector3(0,VERTICAL_PADDING,0);
        interactableUI.display(interactableDisplayType.getKey(),text);
    }
    public void hide() {
        displayedInteractable = null;
        interactableUI.hide();
    }
}
