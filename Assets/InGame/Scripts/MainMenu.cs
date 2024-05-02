using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{


    public void Multiplayer()
    {
        SceneManager.LoadSceneAsync("InGame/Scenes/LoadingScreen");
    }

    public void Quit()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void Feedback()
    {
        SceneManager.LoadSceneAsync("InGame/Scenes/Feedback");
    }

    public void Menu()
    {
        SceneManager.LoadSceneAsync("InGame/Scenes/MainMenu");
    }

}
