using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractableObject : MonoBehaviour
{
    private bool isInteracting = false;
    private bool isInteractedWith = false;

    public int objectRequirement;
    
    // Jacob says hi
    public TextAsset textFile;
    public Text objectText;
    private PlayerMovement playerMove;
    private MouseLook mouseControl;
    public GameObject InteractableBox;
    //public DialogueManager cancelmovement;

    // Start is called before the first frame update
    private void Start()
    {
        InteractableBox.SetActive(false);
    }

    private void DisplayObjectInfo()
    {
        if (!isInteractedWith)
        {
            ChangeStatus();
            isInteractedWith = true;
        }
        // Code for displaying object info goes here.
        // Will require new UI with a text box.
        // Will preferabbly use a text file for reading the displayed info.
        // Refer to DialogueManager.cs & DialogueTrigger.cs and my example dialogue files for help in reading text.
        // Ask me if you have questions.

        InteractableBox.SetActive(true);
        string text = textFile.text;
        objectText.text = text;
        //cancelmovement.DisableCameraControl();
        DisablePlayerControl();
        
    }

    private void DisablePlayerControl()
    {
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerMove.Idle();
        playerMove.enabled = false;
        mouseControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>();
        mouseControl.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void EnablePlayerControl()
    {
        playerMove.enabled = true;
        mouseControl.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void ChangeStatus()
    {
        FindObjectOfType<DialogueManager>().AddRequiredObjects(objectRequirement);
    }

    private void EndInteraction()
    {
        isInteracting = false;
        isInteractedWith = true;
        objectText.text = "";
        InteractableBox.SetActive(false);
        EnablePlayerControl();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKey(KeyCode.E) && !isInteracting)
        {
            isInteracting = true;
            DisplayObjectInfo();
            Debug.Log("Interacted");
            
        }
        if(other.tag == "Player" && Input.GetKey(KeyCode.Space) && isInteracting)
        {
            EndInteraction();
        }
    }
}
