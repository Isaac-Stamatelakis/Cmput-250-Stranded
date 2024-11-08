using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    public void DropItem() {
        GameObject instantiated = GameObject.Instantiate(prefab);
        instantiated.transform.position = transform.position;
    }
}
