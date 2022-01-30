using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TMPro_DialogueManager : MonoBehaviour
{
    public GameObject DialogueBox;

    public TMP_Text BodyText;
    public TMP_Text NameText;

    [Space(20)]
    public GameObject ChoicesBox;

    public GameObject[] ChoiceButtons;
    public TMP_Text[] ChoiceTexts;

    [Space(20)]
    public bool freezePlayerOnDialogue = true;

    private string choiceMade;
    private string choiceNumber;

    private int resultLineAmt = 0;

    private bool isChoosing = false;
    private bool isDialogue = false;

    private string[] dialogueText;

    private Queue<string> inputStream = new Queue<string>();

    private PlayerMovement playerMove;
    private MouseLook mouseControl;

    private List<string> requiredObjects = new List<string>();

    // Start is called before the first frame update
    private void Start()
    {
        DialogueBox.SetActive(false);
        ChoicesBox.SetActive(false);
        for (int x = 0; x < 4; x++)
        {
            ChoiceButtons[x].SetActive(false);
        }
    }

    private void DisablePlayerMovement()
    {
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerMove.enabled = false;
        playerMove.Idle();
    }

    private void EnablePlayerMovement()
    {
        playerMove.enabled = true;
    }

    private void DisableCameraControl()
    {
        mouseControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>();
        mouseControl.enabled = false;
    }

    private void EnableCameraControl()
    {
        mouseControl.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void StartDialogue(Queue<string> dialogue)
    {
        if (freezePlayerOnDialogue)
        {
            DisablePlayerMovement();
            DisableCameraControl();
        }

        isDialogue = true;

        DialogueBox.SetActive(true);
        inputStream = dialogue;
        PrintDialogue();
    }

    public bool AdvanceDialogue()
    {
        if (!isChoosing && isDialogue)
        {
            if (inputStream.Peek().Contains("[LOADSCENE=") || inputStream.Peek().Contains("EndQueue"))
            {
                PrintDialogue();
                return true;
            }
            else
            {
                Debug.Log("Printing as per normal");
                PrintDialogue();
                return false;
            }
        }
        else
        {
            Debug.Log("Not doing anything");
            return true;
        }
    }

    private void PrintDialogue()
    {
        if (inputStream.Peek().Contains("[LOADSCENE="))
        {
            ShouldLoadScene();
        }
        else if (inputStream.Peek().Contains("EndQueue"))
        {
            inputStream.Dequeue();
            EndDialogue();
        }
        else if (inputStream.Peek().Contains("[NAME="))
        {
            string name = inputStream.Peek();
            name = inputStream.Dequeue().Substring(name.IndexOf('=') + 1, name.IndexOf(']') - (name.IndexOf('=') + 1));
            NameText.text = name;
            PrintDialogue();
        }
        else if (inputStream.Peek().Contains("**Choices"))
        {
            inputStream.Dequeue();
            PrintChoices();
        }
        else if (inputStream.Peek().Contains("[CHOICE="))
        {
            PrintChoices();
        }
        else if (inputStream.Peek().Contains("["))
        {
            if (inputStream.Peek().Contains("[" + choiceMade))
            {
                inputStream.Dequeue();
                resultLineAmt = int.Parse(inputStream.Dequeue().Substring(1, 1));
                BodyText.text = inputStream.Dequeue();
                resultLineAmt = 0;
            }
            else
            {
                inputStream.Dequeue();
                resultLineAmt = int.Parse(inputStream.Dequeue().Substring(1, 1));
                inputStream.Dequeue();
                resultLineAmt -= 1;
                PrintDialogue();
            }
        }
        else if (resultLineAmt > 0)
        {
            inputStream.Dequeue();
            PrintDialogue();
        }
        else
        {
            BodyText.text = inputStream.Dequeue();
        }
    }

    private void PrintChoices()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        ChoicesBox.SetActive(true);
        isChoosing = true;
        for (int x = 0; x < 4; x++)
        {
            if (inputStream.Peek().Contains("[CHOICE=") || inputStream.Peek().Contains("[*"))
            {
                //Debug.Log("Pass " + x.ToString());
                //Debug.Log(inputStream.Peek() + " Checking...");
                if (CheckChoice())
                {
                    //Debug.Log(inputStream.Peek() + "Checking for [*");
                    if (inputStream.Peek().Contains("[*"))
                    {
                        x--;
                    }
                    else
                    {
                        //Debug.Log("Checks Out.");
                        //Debug.Log(inputStream.Peek() + " 1st peek");
                        ChoiceButtons[x].SetActive(true);
                        //Debug.Log(inputStream.Peek() + " 2nd peek");
                        ChoiceButtons[x].GetComponent<ButtonChoice>().ChangeChoice(choiceNumber);
                        ChoiceTexts[x].text = inputStream.Dequeue();
                        //Debug.Log(inputStream.Peek() + " Checking again...");
                    }
                }
                else
                {
                    Debug.Log("Doesn't check out.");
                    x--;
                    if (!inputStream.Peek().Contains("[*"))
                    {
                        Debug.Log(inputStream.Dequeue());
                    }
                }
            }
        }
        Debug.Log("Finished printing choices");
    }

    private bool CheckChoice()
    {
        if (inputStream.Peek().Contains("[CHOICE="))
        {

            //Debug.Log("Removing");
            choiceNumber = inputStream.Dequeue().Substring(8, 1);
            //Debug.Log(choiceNumber);
        }

        if (inputStream.Peek().Contains("[*"))
        {
            //Debug.Log("Check Phase 1");
            //Debug.Log(inputStream.Peek().Substring(3, 1) + "Check Phase 2");
            //Debug.Log(inputStream.Peek().Substring(2, 1) + "Check Phase 3");
            if (inputStream.Peek().Contains("[*!"))
            {
                if (requiredObjects.Contains(inputStream.Dequeue().Substring(3, 1)))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (requiredObjects.Contains(inputStream.Dequeue().Substring(2, 1)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            Debug.Log("No requirements");
            return true;
        }
    }

    private void ShouldLoadScene()
    {
        string sceneIndex = inputStream.Dequeue().Substring(11, 1);
        if (inputStream.Peek().Contains("[*"))
        {
            string requirement = inputStream.Dequeue().Substring(2, 1);
            if (requirement == choiceMade)
            {
                EndDialogue();
                FindObjectOfType<SceneLoader>().ChangeScene(int.Parse(sceneIndex));
            }
        }
        else
        {
            EndDialogue();
            FindObjectOfType<SceneLoader>().ChangeScene(int.Parse(sceneIndex));
        }
    }

    public void MadeChoice(int choice)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        FindObjectOfType<ResultsCarrier>().RetrieveResults(choice);
        choiceMade = choice.ToString();
        ChoicesBox.SetActive(false);
        PrintDialogue();
        isChoosing = false;
        requiredObjects.Clear();
    }

    public void AddRequiredObjects(int tempObject)
    {
        requiredObjects.Add(tempObject.ToString());
    }

    public void EndDialogue()
    {
        if (isDialogue)
        {
            Debug.Log("Ending Dialogue");
            inputStream.Clear();
            BodyText.text = "";
            NameText.text = "";
            inputStream.Clear();
            DialogueBox.SetActive(false);
            for (int x = 0; x < 4; x++)
            {
                ChoiceButtons[x].SetActive(false);
            }

            if (freezePlayerOnDialogue)
            {
                EnablePlayerMovement();
            }

            if (mouseControl.enabled == false)
            {
                mouseControl.enabled = true;
            }
            isDialogue = false;
        }
    }
}
