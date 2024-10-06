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
                return "T";
        }
        throw new System.Exception($"Did not cover case for {interactableDisplayType}");
    }
}
public class InteractableUIController : MonoBehaviour
{
    [SerializeField] private InteractableUI interactableUI;
    private static InteractableUIController instance;
    public static InteractableUIController Instance => instance;
    private InteractableDisplayType? displayedInteractable;
    public void Awake() {
        instance = this;
    }

    public void display(InteractableDisplayType interactableDisplayType, string text) {
        bool displaying = displayedInteractable != null;
        if (displaying) {
            bool lessPriorityOrEqual = interactableDisplayType <= displayedInteractable;
            if (lessPriorityOrEqual) {
                return;
            }
        }
        interactableUI.display(interactableDisplayType.getKey(),text);
    }
    public void hide() {
        interactableUI.hide();
    }
}
