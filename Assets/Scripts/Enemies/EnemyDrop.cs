using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    public void OnDestroy() {
        GameObject.Instantiate(prefab);
    }
}
