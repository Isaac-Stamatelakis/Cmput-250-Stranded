using UnityEngine;
using TMPro;

public class PlayerTutorialManager : MonoBehaviour
{
    public TextMeshProUGUI firstInstructionText;  
    public TextMeshProUGUI secondInstructionText; 
    public TextMeshProUGUI thirdInstructionText;  
    public GameObject dialogueTutorial;          

    private bool waitingForMovement = true;
    private bool waitingForRun = false;
    private bool waitingForThirdInstruction = false;

    void Start()
    {
        ShowFirstInstruction();
    }

    void Update()
    {
        if (waitingForMovement && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) ||
                                   Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
                                   Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            waitingForMovement = false;
            ShowSecondInstruction();
        }

        if (waitingForRun && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            waitingForRun = false;
            ShowThirdInstruction();  
        }

        if (waitingForThirdInstruction && Input.GetKeyDown(KeyCode.Space))
        {
            HideAllInstructions();  
        }
    }

    void ShowFirstInstruction()
    {
        firstInstructionText.gameObject.SetActive(true);   
        secondInstructionText.gameObject.SetActive(false);
        thirdInstructionText.gameObject.SetActive(false);  
        waitingForMovement = true;
    }

    void ShowSecondInstruction()
    {
        firstInstructionText.gameObject.SetActive(false);  
        secondInstructionText.gameObject.SetActive(true);  
        waitingForRun = true;
    }

    void ShowThirdInstruction()
    {
        secondInstructionText.gameObject.SetActive(false);  
        thirdInstructionText.gameObject.SetActive(true);    
        waitingForThirdInstruction = true;                 
    }

    void HideAllInstructions()
    {
        firstInstructionText.gameObject.SetActive(false);
        secondInstructionText.gameObject.SetActive(false);
        thirdInstructionText.gameObject.SetActive(false);  

        if (dialogueTutorial != null)
        {
            dialogueTutorial.SetActive(false); 
        }
    }
}
