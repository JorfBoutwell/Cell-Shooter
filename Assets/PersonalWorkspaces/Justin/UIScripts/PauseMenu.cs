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
        }
        else
        {
            pauseCanvas.SetActive(false);
            pauseActive = false;
            playerManagerScript.inputActions.Enable();
            
            
        }

    }

   

}
