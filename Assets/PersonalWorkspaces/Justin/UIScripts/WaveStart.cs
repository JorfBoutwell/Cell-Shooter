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

    public GameObject playerManager;
    private PlayerManager playerManagerScript;

    //Countdown variables
    [SerializeField] TextMeshPro countdownText;
    public GameObject countdownUnderline;
    public GameObject countdownOverlay;
    public float currentTime = 0f;
    public float countdownTime = 10f;
    public bool startCountdown = false;

    //Healthbar variables
    public GameObject healthBar;
    public GameObject healthBarShadow;
    public float health = 680;
    public float healthShadow = 680;
    //public float healthShadow = 100;

    float damageTaken = 100f;

    //Death variables
    public bool isDead = false;

    void Start()
    {
        currentTime = countdownTime;
        StartCountdown();
        playerManagerScript = playerManager.GetComponent<PlayerManager>();
    }

    void Update()
    {
        //Game Countdown
        if (startCountdown) {
            currentTime -= 1 * Time.deltaTime;
            countdownText.text = currentTime.ToString("0");

            //Activates Final Countdown Overlay and Changes
            if (currentTime <= 3)
            {
                countdownOverlay.SetActive(true);
                countdownText.color = Color.red;
                countdownText.fontSize = 50;
                countdownText.transform.position = new Vector3(0f, 1f, 0f);
            }

            //Deactivats Final Countdown Overlay and Countdown
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

        //Health UI Damage
        if (Input.GetKeyDown(KeyCode.L)) //For Testing
        {
            playerManagerScript.health -= damageTaken;
            healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(playerManagerScript.health, 90);
        }

        if (healthShadow > playerManagerScript.health)
        {
            healthBarShadow.GetComponent<RectTransform>().sizeDelta = new Vector2(healthShadow, 90);
            healthShadow -= 100 * Time.deltaTime;
        }

        if (playerManagerScript.health <= 0)
        {
            playerManagerScript.isDead = true;
        }
    }

    //Resets Countdown to Start Game Timer
    private void Reset()
    {
        currentTime = 180f;
        StartCountdown();
        countdownText.color = Color.white;
        countdownText.fontSize = 12.5f;
        countdownText.transform.position = new Vector3(0f, 4.4f, 0f);
    }

    //Starts Countdown/Game Timer
    public void StartCountdown()
    {
        Debug.Log(currentTime);

        startCountdown = true;

    }

}


