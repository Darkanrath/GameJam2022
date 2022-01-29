using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private GameObject player;
    private Transform oldPlayerPos;

    private PlayerMovement playerMove;
    private MouseLook mouseControl;

    private int currentSceneIndex = 0;
    private int nextSceneIndex;

    private int officeIndex = 1;
    private bool officeLoadedOnce = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void DisablePlayer()
    {
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerMove.enabled = false;
        playerMove.Idle();
        mouseControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>();
        mouseControl.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        player.SetActive(false);
    }

    private void EnablePlayer()
    {
        player.SetActive(true);
        playerMove.enabled = true;
        mouseControl.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ChangeScene(int index)
    {
        DisablePlayer();

        if (index == officeIndex && officeLoadedOnce)
        {
            Debug.Log("Going back to Office");
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(index));
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(currentSceneIndex));
            player = GameObject.FindGameObjectWithTag("Player");
            FindObjectOfType<DialogueManager>().ChangeAnObject();
            EnablePlayer();
        }
        else
        {
            if (index == officeIndex && !officeLoadedOnce)
            {
                Debug.Log("Entering Office for first time");
                StartCoroutine(LoadSceneAsync(index));
            }
            else
            {
                Debug.Log("Heading to next scene");
                currentSceneIndex = index;
                StartCoroutine(LoadSceneAsync(index));
            }
        }
    }

    IEnumerator LoadSceneAsync(int index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        //Debug.Log(player.transform.position);
        if (!officeLoadedOnce)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(currentSceneIndex));
            //Debug.Log("Loaded Office Scene");
            if (index == officeIndex)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                FindObjectOfType<DialogueManager>().AddColorableObjects();
                officeLoadedOnce = true;
                EnablePlayer();
            }
        }
        //Debug.Log(player.transform.position);
    }
}
