using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObjectGameObject<T> : InteractableGameObject where T : InteractableObject
{
    [SerializeField] protected T interactableObject;
    public override void Start() {
        base.Start();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = interactableObject.getSprite();
    }
}
