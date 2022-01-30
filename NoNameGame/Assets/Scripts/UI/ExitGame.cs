using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    private bool triggeredOnce = false;

    private GameObject exitUI;

    private void Start()
    {
        exitUI = GameObject.FindGameObjectWithTag("ExitUI");
        exitUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !triggeredOnce)
        {
            StartCoroutine(TriggerCountDown());
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && triggeredOnce)
        {
            Application.Quit();
        }
    }

    IEnumerator TriggerCountDown()
    {
        triggeredOnce = true;
        exitUI.SetActive(true);
        yield return new WaitForSeconds(2);
        exitUI.SetActive(false);
        triggeredOnce = false;
    }
}
