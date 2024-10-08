using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractableGameObject {
    public void interact();
    public void highlight();
    public void unhighlight();
    public string getInteractText();
    public Vector3 getPosition();
    public bool isInteractable();
}
public abstract class InteractableGameObject : MonoBehaviour, IInteractableGameObject
{
    
    [SerializeField] protected Material highlightShader;
    protected Material defaultMaterial;
    protected bool interactable = true;
    public virtual void Start() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
    }
    public abstract void interact();

    public virtual void highlight()
    {
        if (highlightShader != null) {
            GetComponent<SpriteRenderer>().material = highlightShader;
        }
        
    }

    public virtual void unhighlight()
    {
        if (highlightShader != null) {
            GetComponent<SpriteRenderer>().material = defaultMaterial;
        }
    }

    public abstract string getInteractText();

    public Vector3 getPosition()
    {
        return transform.position;
    }

    public virtual bool isInteractable()
    {
        return interactable;
    }
}
