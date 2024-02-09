using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class WaveStart : MonoBehaviour
{
    //Start variables
    static public bool playersReady;
    static public bool enemySpawnActive;
    static public bool pickUpSpawn;
    public bool gameStart;

    //Countdown variables
    public GameObject countdownText;
    public TextMeshProUGUI countdownTimer;
    public GameObject countdownUnderline;
    public GameObject countdownOverlay;
    public float currentTime;
    public float countdownTime = 10f;
    public float gameTimeSeconds;
    public float gameTimeMinutes;
    public bool startCountdown = false;
    public bool gameTimerStart = false;

    //Objective variables
    public List<string> objectiveTextPrompts = new List<string>();
    public GameObject objectiveText;
    public GameObject objectiveTextLine;
    int objectiveTextValue;

    void Start()
    {
        currentTime = countdownTime;
        StartCountdown();
        countdownTimer = countdownText.GetComponentInChildren<TextMeshProUGUI>();
        PlayerManager playerManagerScript = GetComponentInParent<PlayerManager>();

        objectiveText.SetActive(false);
        objectiveTextLine.SetActive(false);

        
        //Objective Text Prompts
        objectiveTextPrompts.Add("Your team needs to reach 1000 points to win!");
        objectiveTextPrompts.Add("Earn points by claiming buttons around the map!");
        objectiveTextPrompts.Add("Simply touch a button to claim it!");
        objectiveTextPrompts.Add("Enemy players can claim your buttons instantly!");
        objectiveTextPrompts.Add("If you die, all your buttons will be unclaimed!");
        objectiveTextPrompts.Add("You can't wall jump forever!");

    }

    void Update()
    {
        //Game Countdown
        if (startCountdown) {
            currentTime -= 1 * Time.deltaTime;
            countdownTimer.text = currentTime.ToString("0");
            if(gameTimerStart)
            {
                gameTimeMinutes = Mathf.FloorToInt(currentTime / 60);
                gameTimeSeconds = Mathf.FloorToInt(currentTime % 60);
                countdownTimer.text = string.Format("{0:00}:{1:00}", gameTimeMinutes, gameTimeSeconds);
            }

            //Activates Final Countdown Overlay and Changes
            if (currentTime <= 3)
            {
                countdownOverlay.SetActive(true);
                countdownTimer.color = Color.red;
                countdownTimer.fontSize = 200;
                countdownText.transform.position = new Vector3(1000f, 200f, 0f);
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
        countdownTimer.color = Color.white;
        countdownTimer.fontSize = 55f;
        countdownText.transform.position = new Vector3(958f, 540f, 0f);
    }

    //Starts Countdown/Game Timer
    public void StartCountdown()
    {
        startCountdown = true;

        StartCoroutine("ObjectiveEnter");

    }

    //Displays Objective Text
    IEnumerator ObjectiveEnter()
    {
        objectiveText.GetComponentInChildren<TextMeshProUGUI>().DOFade(0, 1);

        objectiveText.SetActive(true);
        objectiveTextLine.SetActive(true);

        for (int i = 0; i < objectiveTextPrompts.Count; i++)
            {
            //objectiveText.GetComponentInChildren<TextMeshProUGUI>().text = objectiveTextPrompts[Random.Range(0, objectiveTextPrompts.Count)];
            objectiveText.GetComponentInChildren<TextMeshProUGUI>().text = objectiveTextPrompts[i];

            objectiveText.GetComponentInChildren<TextMeshProUGUI>().DOFade(1, 1);
            //objectiveTextLine.GetComponentInChildren<Material>().DOFade(1, 1);

            yield return new WaitForSeconds(2.75f);

            objectiveText.GetComponentInChildren<TextMeshProUGUI>().DOFade(0, 1);

            if(i == objectiveTextPrompts.Count - 1)
            {
                yield return new WaitForSeconds(1f);
                objectiveText.SetActive(false);
                objectiveTextLine.SetActive(false);
            }
            //objectiveTextLine.GetComponentInChildren<Material>().DOFade(0, 1);

            yield return new WaitForSeconds(2.75f);

        }
        
    }

}


