using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Controller : MonoBehaviour
{
    public int levelCount;

    public void exitApp()
    {
        Application.Quit();
    }

    public void loadNextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex< levelCount - 1)
        {
            Scene activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.buildIndex + 1, LoadSceneMode.Single);
        }
    }

   
}
