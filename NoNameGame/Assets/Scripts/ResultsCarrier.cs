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

    }
}
