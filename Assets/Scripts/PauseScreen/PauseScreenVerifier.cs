using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseScreenVerifier : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button cancel;
    [SerializeField] private Button countinue;

    public delegate void VerifierCallBack();
    public void display(VerifierCallBack callBack, string displayText, string cancelText, string coutinueText) {
        this.text.text = displayText;
        cancel.onClick.AddListener(() => {
            GameObject.Destroy(gameObject);
        });
        countinue.onClick.AddListener(() => {
            callBack();
            GameObject.Destroy(gameObject);
        });
        if (cancelText != null) {
            cancel.GetComponentInChildren<TextMeshProUGUI>().text = cancelText;
        }
        if (coutinueText != null) {
            countinue.GetComponentInChildren<TextMeshProUGUI>().text = coutinueText;
        }
        
    }
}
