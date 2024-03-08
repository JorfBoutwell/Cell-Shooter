using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas;
    public bool pauseActive = false;
    public PlayerManager playerManagerScript;

    void Start()
    {
        
    }

    public void menuOnOff()
    {

        if (!pauseActive)
        {
            Debug.Log("Mind");
            playerManagerScript.inputActions.Disable();
            playerManagerScript.inputActions.Menu.Enable();
            pauseCanvas.SetActive(true);
            pauseActive = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            pauseCanvas.SetActive(false);
            pauseActive = false;
            playerManagerScript.inputActions.Enable();
        }

        if (!pauseActive && Cursor.visible == true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

   

}
