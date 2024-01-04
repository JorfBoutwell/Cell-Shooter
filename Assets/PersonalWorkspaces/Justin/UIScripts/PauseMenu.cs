using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas;
    public bool pauseActive = false;

    void Start()
    {
        
    }

    //Activates Pause Menu
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseActive) { 
                pauseCanvas.SetActive(true);
                pauseActive = true;
            }
            else
            {
                pauseCanvas.SetActive(false);
                pauseActive = false;
            }
        }

        
    }
}
