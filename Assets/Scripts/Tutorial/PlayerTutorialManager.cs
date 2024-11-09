using UnityEngine;
using TMPro;
using Dialogue;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using PlayerModule;
using Rooms;

public class PlayerTutorialManager : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    private bool waitingForMovement = true;
    private bool waitingForRun = false;
    private bool waitingForThirdInstruction = false;
    private int index;
    private List<string> text = new List<string>{
        "Use The ARROW KEYS \nor WASD to Move!",
        "Hold SHIFT to Run!",
        "Pick up a Weapon!",
        "Left Click to Attack!",
        "Press ESC to Pause!",
        "Go Through the Door!"
    };

    void Update()
    {
        textMeshProUGUI.text = text[index];
        switch (index) {
            case 0:
                if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || 
                    Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || 
                    Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
                    Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)
                )) {
                    index ++;
                }
                break;
            case 1:
                if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) {
                    index ++;
                }
                break;
            case 2:
                if (Player.Instance.GetComponent<PlayerAttack>().currentWeapon != null) {
                    index ++;
                }
                break;
            case 3:
                if (Input.GetMouseButtonDown(0)) {
                    index++;
                }
                break;
            case 4:
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    index ++;
                }
                break;
            case 5:
                if (!LevelManager.getInstance().CurrentLevel.CurrentRoomClear()) {
                    index++;
                }
                break;
        }
        if (index >= text.Count) {
            GameObject.Destroy(gameObject);
        }

    }
}
