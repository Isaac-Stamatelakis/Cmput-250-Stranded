using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZombieTimer : MonoBehaviour
{

    [SerializeField] private GameObject BossZoms;
    // Start is called before the first frame update
    void Start()
    {
        BossZoms.SetActive(false);
        StartCoroutine(timer());
    }

    IEnumerator timer() {
        yield return new WaitForSeconds(2);
        BossZoms.SetActive(true);
    }
}
