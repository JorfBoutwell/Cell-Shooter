using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas;
    public bool pauseActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
