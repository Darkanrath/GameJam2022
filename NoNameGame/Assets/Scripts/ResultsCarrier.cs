using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsCarrier : MonoBehaviour
{
    private int choiceMade;

    public void RetrieveResults(int result)
    {
        choiceMade = result;
    }

    public void ChangeSceneSpecifics()
    {
        if (GameObject.FindGameObjectWithTag("ChangeByResult"))
        {
            GameObject toChange = GameObject.FindGameObjectWithTag("ChangeByResult");
            toChange.GetComponent<TMPro_DialogueTrigger>().interactAmt = choiceMade - 1;
            toChange.GetComponent<TMPro_DialogueTrigger>().moreDialogue = false;
        }
    }
}
