using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveStart : MonoBehaviour
{
    static public bool playersReady;
    static public bool enemySpawnActive;
    static public bool pickUpSpawn;
    //public TextMeshPro countdownText;
    [SerializeField] TextMeshPro countdownText;
    public TextMeshPro bulletCount;

    public float currentTime = 0f;
    public float countdownTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = countdownTime;
        StartCountdown();
    }

    // Update is called once per frame
    void Update()
    {
        /*while (countdownTime == 10 && countdownTime >= 0)
        {
            countdownText.text = countdownTime.ToString("0");
            countdownTime -= Time.deltaTime;
            Debug.Log(countdownTime);
        }*/
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
        //float time = 0f;


        /*for (float j = 0; j < 10; j += time)
        {
            time += Time.deltaTime;
            countdownText.text = countdownTime.ToString("0");
            countdownTime -= 1;
            Debug.Log(countdownTime);

            if (countdownTime == 0) { 
            break;
            }
        }*/
        
    }
}


