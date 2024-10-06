using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractableObject {
    public Sprite getSprite();
    public string getName();
}
[CreateAssetMenu(fileName = "New Interactable Object", menuName = "Interactables/Object")]
public class InteractableObject : ScriptableObject, IInteractableObject
{
    [SerializeField] private Sprite sprite;

    public string getName()
    {
        return name;
    }

    public Sprite getSprite()
    {
        return sprite;
    }
}
