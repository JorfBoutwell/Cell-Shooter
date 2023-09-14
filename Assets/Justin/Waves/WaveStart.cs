using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveStart : MonoBehaviour
{
    static public bool playersReady;
    static public bool enemySpawnActive;
    static public bool pickUpSpawn;
    public TextMeshPro countdownText;
    public int countdownAmount;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void StartWave()
    {
        if (playersReady)
        {
            enemySpawnActive = true;
            pickUpSpawn = true;
            StartCountdown();
        }
    }

    private void StartCountdown()
    {
        countdownAmount = 10;
        
    }
}


