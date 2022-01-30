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
    private bool isLoading = false;

    public Animator animTransition;

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
        if (isLoading == false)
        {
            isLoading = true;

            if (GameObject.FindGameObjectWithTag("Player"))
            {
                DisablePlayer();
            }

            if (index == officeIndex && officeLoadedOnce)
            {
                Debug.Log("Going back to Office");
                StartCoroutine(GoBackToOffice(index));
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
                    FindObjectOfType<OfficeManager>().HideOffice();
                    StartCoroutine(LoadSceneAsync(index));
                }
            }
        }
    }

    IEnumerator LoadSceneAsync(int index)
    {
        animTransition.Play("crossfade_start");
        yield return new WaitForSeconds(1.5f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        FindObjectOfType<ResultsCarrier>().ChangeSceneSpecifics();
        //Debug.Log(player.transform.position);
        if (!officeLoadedOnce)
        {
            Debug.Log("Why");
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(currentSceneIndex));
            //Debug.Log("Loaded Office Scene");
            if (index == officeIndex)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                officeLoadedOnce = true;
            }
        }
        //Debug.Log(player.transform.position);

        animTransition.Play("crossfade_end");
        yield return new WaitForSeconds(1.5f);

        isLoading = false;
    }

    IEnumerator GoBackToOffice(int index)
    {
        animTransition.Play("crossfade_start");
        yield return new WaitForSeconds(1.5f);

        yield return new WaitForSeconds(1);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(index));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(currentSceneIndex));
        FindObjectOfType<OfficeManager>().ShowOffice();
        FindObjectOfType<OfficeManager>().ChangeOffice();
        player = GameObject.FindGameObjectWithTag("Player");
        FindObjectOfType<OfficeManager>().ChangeAnObject();

        yield return new WaitForSeconds(1);
        EnablePlayer();

        animTransition.Play("crossfade_end");
        yield return new WaitForSeconds(1.5f);

        FindObjectOfType<IntroductionDialogue>().ReRun();

        isLoading = false;
    }
}
