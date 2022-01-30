using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public int sceneIndex;

    private TMPro_InteractManager intManagerRef;

    private bool transitioning = false;

    private void Awake()
    {
        intManagerRef = FindObjectOfType<TMPro_InteractManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            intManagerRef.ShowInteractText("'E' to Open");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKey(KeyCode.E) && !transitioning)
        {
            transitioning = true;
            intManagerRef.HideInteractText();
            Debug.Log("Interacted");
            FindObjectOfType<SceneLoader>().ChangeScene(sceneIndex);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        intManagerRef.HideInteractText();
        transitioning = false;
    }
}
