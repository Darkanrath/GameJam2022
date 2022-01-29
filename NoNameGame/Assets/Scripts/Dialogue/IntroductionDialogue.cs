using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionDialogue : MonoBehaviour
{
    public TextAsset dialogueTextFile;

    private Queue<string> dialogue = new Queue<string>();

    private float waitTime = 0.5f;
    private float nextTime = 0f;

    private bool dialogueTriggered = false;

    private void Start()
    {
        TriggerDialogue();
        dialogueTriggered = true;
    }

    private void TriggerDialogue()
    {
        ReadTextFile();
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    private void ReadTextFile()
    {
        string txt = dialogueTextFile.text;

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
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.name);
        if (other.gameObject.tag == "Player" && Input.GetKey(KeyCode.Space) && nextTime < Time.timeSinceLevelLoad && dialogueTriggered == true)
        {
            nextTime = Time.timeSinceLevelLoad + waitTime;
            FindObjectOfType<DialogueManager>().AdvanceDialogue();
        }
    }
}
