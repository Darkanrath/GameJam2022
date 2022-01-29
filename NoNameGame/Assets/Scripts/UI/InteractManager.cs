using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractManager : MonoBehaviour
{
    public Text objectText;
    public GameObject interactableBox;
    public Scrollbar objectScrollbar;

    [Space(20)]
    public GameObject interactTextBox;
    public Text interactText;

    private PlayerMovement playerMove;
    private MouseLook mouseControl;

    private void Start()
    {
        interactTextBox.SetActive(false);
        interactableBox.SetActive(false);
    }

    private void DisablePlayerControl()
    {
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerMove.enabled = false;
        playerMove.Idle();
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

    public void ShowInteractText(string text)
    {
        interactTextBox.SetActive(true);
        interactText.text = text;
    }

    public void HideInteractText()
    {
        interactTextBox.SetActive(false);
        interactText.text = "";
    }

    public void DisplayObjectInfo(string bodyText)
    {
        interactTextBox.SetActive(false);
        interactText.text = "";
        interactableBox.SetActive(true);
        objectText.text = bodyText;
        //cancelmovement.DisableCameraControl();
        DisablePlayerControl();
    }

    public void EndInteraction()
    {
        objectText.text = "";
        objectScrollbar.value = 1;
        interactableBox.SetActive(false);
        EnablePlayerControl();
    }
}
