using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNextScene : MonoBehaviour
{
    public int sceneIndex;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindObjectOfType<SceneLoader>().ChangeScene(sceneIndex);
        }
    }
}
