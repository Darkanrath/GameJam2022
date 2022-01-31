using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TMPro_DialogueTrigger : MonoBehaviour
{
    public TextAsset[] dialogueTextFile;

    public string interactionText;

    [Space(20)]
    public int objectRequirement;

    [Space(10)]
    public bool colorAnObject = false;

    [HideInInspector]
    public int interactAmt = 0;
    [HideInInspector]
    public bool moreDialogue = true;

    private GameObject interactTextBox;
    private TMP_Text interactText;

    private Queue<string> dialogue = new Queue<string>();

    private float waitTime = 0.8f;
    private float nextTime = 0f;

    private bool dialogueTriggered = false;
    private bool isInteractedWith = false;

    private TMPro_InteractManager intManagerRef;

    private void Awake()
    {
        intManagerRef = FindObjectOfType<TMPro_InteractManager>();
    }

    private void TriggerDialogue()
    {
        ReadTextFile();
        FindObjectOfType<TMPro_DialogueManager>().StartDialogue(dialogue);
        if (!isInteractedWith)
        {
            ChangeStatus();
            if (colorAnObject)
            {
                FindObjectOfType<OfficeManager>().ChangeAnObject();
            }
            isInteractedWith = true;
        }
    }

    private void ReadTextFile()
    {
        string txt = dialogueTextFile[interactAmt].text;

        string[] lines = txt.Split(System.Environment.NewLine.ToCharArray());

        foreach (string line in lines)
        {
            if (!string.IsNullOrEmpty(line))
            {
                if (line.StartsWith("["))
                {
                    string special = line.Substring(0, line.IndexOf(']') + 1);
                    string curr = line.Substring(line.IndexOf(']') + 1);
                    dialogue.Enqueue(special);
                    dialogue.Enqueue(curr);
                }
                else
                {
                    dialogue.Enqueue(line);
                }
            }
        }
        dialogue.Enqueue("EndQueue");

        if (interactAmt != dialogueTextFile.Length - 1 && moreDialogue == true)
        {
            interactAmt += 1;
        }
    }

    private void ChangeStatus()
    {
        FindObjectOfType<TMPro_DialogueManager>().AddRequiredObjects(objectRequirement);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && dialogueTriggered == false)
        {
            intManagerRef.ShowInteractText(interactionText);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && dialogueTriggered == false && Input.GetKey(KeyCode.E))
        {
            dialogueTriggered = true;
            TriggerDialogue();
            intManagerRef.HideInteractText();
            //Debug.Log("Collision");
        }

        //Debug.Log(other.name);
        if (other.gameObject.tag == "Player" && Input.GetKey(KeyCode.Space) && nextTime < Time.timeSinceLevelLoad && dialogueTriggered == true)
        {
            nextTime = Time.timeSinceLevelLoad + waitTime;
            FindObjectOfType<TMPro_DialogueManager>().AdvanceDialogue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && dialogueTriggered == true)
        {
            FindObjectOfType<TMPro_DialogueManager>().EndDialogue();
            dialogueTriggered = false;
        }
        intManagerRef.HideInteractText();
    }
}
