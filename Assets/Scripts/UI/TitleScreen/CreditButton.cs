using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CreditButton : MonoBehaviour
{
    [SerializeField] private GameObject credits;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            credits.gameObject.SetActive(true);
        });
    }

    
}
