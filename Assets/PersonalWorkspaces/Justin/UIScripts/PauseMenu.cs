using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas;
    public bool pauseActive = false;
    public PlayerManager playerManagerScript;
    GameObject background;
    GameObject settings;
    GameObject buttons;
    GameObject feedback;

    void Start()
    {
        background = pauseCanvas.transform.GetChild(0).gameObject;
        settings = pauseCanvas.transform.GetChild(1).gameObject;
        buttons = pauseCanvas.transform.GetChild(2).gameObject;
        feedback = pauseCanvas.transform.GetChild(3).gameObject;
    }

    public void menuOnOff()
    {

        if (!pauseActive)
        {
            playerManagerScript.inputActions.Disable();
            playerManagerScript.inputActions.Menu.Enable();

            pauseCanvas.SetActive(true);
            pauseActive = true;

            background.SetActive(true);
            buttons.SetActive(true);
            settings.SetActive(false);
            feedback.SetActive(false);

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
