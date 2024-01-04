using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void Multiplayer()
    {
        SceneManager.LoadSceneAsync("InGame/Scenes/Lobby");
    }

    public void Quit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
