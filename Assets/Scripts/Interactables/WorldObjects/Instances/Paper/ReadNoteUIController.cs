using System;
using System.Collections;
using System.Collections.Generic;
using PlayerModule;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ReadNoteUIController : MonoBehaviour
{
    private static ReadNoteUIController instance;
    public static ReadNoteUIController Instance => instance;
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI content;
    public void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void Start()
    {
        
    }

    public void Display(string header, string content)
    {
        Player.Instance.setDialog(true);
        gameObject.SetActive(true);
        this.header.text = header;
        this.content.text = content;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
        {
            gameObject.SetActive(false);
            Player.Instance.setDialog(false);
        }
    }
}
