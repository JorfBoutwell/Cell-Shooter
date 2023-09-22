using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveStart : MonoBehaviour
{
    static public bool playersReady;
    static public bool enemySpawnActive;
    static public bool pickUpSpawn;

    [SerializeField] TextMeshPro countdownText;
    public GameObject countdownOverlay;
    public TextMeshPro bulletCount;

    public float currentTime = 0f;
    public float countdownTime = 10f;

    public bool startCountdown = false;
    public bool gameStart;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = countdownTime;
        StartCountdown();
    }

    // Update is called once per frame
    void Update()
    {
        if (startCountdown) { 
            currentTime -= 1 * Time.deltaTime;
            countdownText.text = currentTime.ToString("0");
            if(currentTime <= 3)
            {
                //countdownText.enabled = false;
                countdownOverlay.SetActive(true);
                countdownText.color = Color.red;
                countdownText.fontSize = 50;
                countdownText.transform.position = new Vector3(9f, .5f, 0);
            }
            if (currentTime <= 0)
            {
                currentTime = 0;
                startCountdown = false;
            }
        }
    }

    private void Reset()
    {
        //Countdown
        currentTime = 10f;
        //EnemyDespawn
    }

    private void StartEnemySpawn()
    {
        if(enemySpawnActive)
        {
            
        }
    }

    private void StartPickUpSpawn()
    {
        if(pickUpSpawn)
        {

        }
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

    public void StartCountdown()
    {
        Debug.Log(currentTime);

        startCountdown = true;

        /*for (float j = 10f; j >= 0; j -= 1 * Time.deltaTime)
        {
            
            countdownText.text = j.ToString("0");
            countdownTime -= 1;
            Debug.Log(countdownTime);

            if (countdownTime == 0) { 
            break;
            }
        }*/
        
    }
}


