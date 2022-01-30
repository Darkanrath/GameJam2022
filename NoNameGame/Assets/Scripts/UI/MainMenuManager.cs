using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour

//Created by Lukas Reeves//

{

    public void PlayGame()
    {
        Debug.Log("Loading Game");
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Closing Game");
        Application.Quit();
    }

}
