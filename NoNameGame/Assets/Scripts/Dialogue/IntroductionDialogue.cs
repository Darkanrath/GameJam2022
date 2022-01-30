using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionDialogue : MonoBehaviour
{
    public List<TextAsset> dialogueTextFile = new List<TextAsset>();

    private Queue<string> dialogue = new Queue<string>();

    private int fileToRun = 0;

    private float waitTime = 0.5f;
    private float nextTime = 0f;

    private bool dialogueTriggered = false;

    private void Start()
    {
        TriggerDialogue();
        dialogueTriggered = true;
    }

    public void ReRun()
    {
        TriggerDialogue();
    }

    private void TriggerDialogue()
    {
        ReadTextFile();
        FindObjectOfType<TMPro_DialogueManager>().StartDialogue(dialogue);
    }

    private void ReadTextFile()
    {
        string txt = dialogueTextFile[fileToRun].text;

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

        if (fileToRun < (dialogueTextFile.Count - 1))
        {
            fileToRun += 1;
        }
    }

    private void Update()
    {
        //Debug.Log(other.name);
        if (Input.GetKey(KeyCode.Space) && nextTime < Time.timeSinceLevelLoad && dialogueTriggered == true)
        {
            nextTime = Time.timeSinceLevelLoad + waitTime;
            FindObjectOfType<TMPro_DialogueManager>().AdvanceDialogue();
        }
    }
}
