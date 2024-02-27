using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas;
    public bool pauseActive = false;
    public bool click = false;
    public PlayerManager playerManagerScript;

    public InputActions inputActions;

    void Start()
    {
        //playerManagerScript = gameObject.GetComponent<PlayerManager>();
        
    }

    //Activates Pause Menu
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            

            if (!pauseActive) {
                Debug.Log("Mind");
                playerManagerScript.inputActions.Disable();

                pauseCanvas.SetActive(true);
                pauseActive = true;
                //click = true;
            }
            else
            {
                pauseCanvas.SetActive(false);
                pauseActive = false;
                playerManagerScript.inputActions.Enable();
            }

            //click = false;
        }

        
    }
}
