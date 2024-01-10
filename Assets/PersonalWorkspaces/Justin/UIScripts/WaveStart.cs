using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveStart : MonoBehaviour
{
    //Start variables
    static public bool playersReady;
    static public bool enemySpawnActive;
    static public bool pickUpSpawn;
    public bool gameStart;

    //Countdown variables
    [SerializeField] TextMeshPro countdownText;
    public GameObject countdownUnderline;
    public GameObject countdownOverlay;
    public float currentTime;
    public float countdownTime = 10f;
    public float gameTimeSeconds;
    public float gameTimeMinutes;
    public bool startCountdown = false;
    public bool gameTimerStart = false;

    void Start()
    {
        currentTime = countdownTime;
        StartCountdown();
<<<<<<< HEAD
=======
        playerManagerScript = GetComponentInParent<PlayerManager>();
>>>>>>> main
    }

    void Update()
    {
        //Game Countdown
        if (startCountdown) {
            currentTime -= 1 * Time.deltaTime;
            countdownText.text = currentTime.ToString("0");
            if(gameTimerStart)
            {
                gameTimeMinutes = Mathf.FloorToInt(currentTime / 60);
                gameTimeSeconds = Mathf.FloorToInt(currentTime % 60);
                countdownText.text = string.Format("{0:00}:{1:00}", gameTimeMinutes, gameTimeSeconds);
            }

            //Activates Final Countdown Overlay and Changes
            if (currentTime <= 3)
            {
                countdownOverlay.SetActive(true);
                countdownText.color = Color.red;
                countdownText.fontSize = 50;
                countdownText.transform.position = new Vector3(0f, 1f, 0f);
            }

            //Deactivates Final Countdown Overlay and Countdown
            if (currentTime <= 0)
            {
                currentTime = 0;
                //startCountdown = false;
                countdownOverlay.SetActive(false);
                //countdownUnderline.SetActive(false);
                //countdownText.enabled = false;
                Reset();
            }
        }
    }

    //Resets Countdown to Start Game Timer
    private void Reset()
    {
        currentTime = 180f;
        gameTimerStart = true;

        StartCountdown();
        countdownText.color = Color.white;
        countdownText.fontSize = 12.5f;
        countdownText.transform.position = new Vector3(0f, 4.4f, 0f);
    }

    //Starts Countdown/Game Timer
    public void StartCountdown()
    {
        startCountdown = true;

    }

}


