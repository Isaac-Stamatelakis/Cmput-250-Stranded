using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractableGameObject {
    public void interact();
    public void highlight();
    public void unhighlight();
    public string getInteractText();
}
public abstract class InteractableGameObject<T> : MonoBehaviour, IInteractableGameObject where T : InteractableObject
{
    [SerializeField] protected T interactableObject;
    [SerializeField] protected Material highlightShader;
    private Material defaultMaterial;
    public virtual void Start() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
        if (spriteRenderer == null) {
            Debug.LogWarning($"Interactable Object {name} has no Sprite Renderer");
        } else {
            spriteRenderer.sprite = interactableObject.getSprite();
        }
    }
    public abstract void interact();

    public void highlight()
    {
        if (highlightShader != null) {
            GetComponent<SpriteRenderer>().material = highlightShader;
        }
        
    }

    public void unhighlight()
    {
        if (highlightShader != null) {
            GetComponent<SpriteRenderer>().material = defaultMaterial;
        }
    }

    public abstract string getInteractText();
}
