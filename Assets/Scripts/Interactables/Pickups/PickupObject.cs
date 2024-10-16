using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;
public interface IPickupObject {
    public void pickup();
}

public abstract class PickupObject : MonoBehaviour, IPickupObject
{
    public float Speed = 1;
    public void pickup() {
        onPickup();
        GameObject.Destroy(gameObject);
    }
    public void OnTriggerStay2D(Collider2D collision2D) {
        Vector2 direction = (Player.Instance.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(Player.Instance.transform.position,transform.position);
        //float realSpeed = Speed / distance;
        transform.Translate(Speed*direction,Space.World);

    }
    protected abstract void onPickup();
}
