using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public TextAsset textFile;

    [Space(10)]
    public string interactionText;

    [Space(20)]
    public int objectRequirement;

    [Space(10)]
    public bool colorAnObject = false;

    private bool isInteracting = false;
    private bool isInteractedWith = false;

    private InteractManager intManagerRef;
    // Jacob says hi

    private void Awake()
    {
        intManagerRef = FindObjectOfType<InteractManager>();
    }

    private void DisplayInfo()
    {
        if (!isInteractedWith)
        {
            ChangeStatus();
            if (colorAnObject)
            {
                FindObjectOfType<DialogueManager>().ChangeAnObject();
            }
            isInteractedWith = true;
        }

        string text = textFile.text;
        intManagerRef.DisplayObjectInfo(text);
    }

    private void ChangeStatus()
    {
        FindObjectOfType<DialogueManager>().AddRequiredObjects(objectRequirement);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            intManagerRef.ShowInteractText(interactionText);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKey(KeyCode.E) && !isInteracting)
        {
            isInteracting = true;
            DisplayInfo();
            Debug.Log("Interacted");
        }
        if(other.tag == "Player" && Input.GetKey(KeyCode.Space) && isInteracting)
        {
            isInteracting = false;
            isInteractedWith = true;
            intManagerRef.EndInteraction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        intManagerRef.HideInteractText();
    }
}
