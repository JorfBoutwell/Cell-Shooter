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

    PlayerManager playerVariables;

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

    float damageTaken = 100f;

    //Death variables
    public bool isDead = false;

    //void Awake() {}
    // Start is called before the first frame update
    void Start()
    {
        currentTime = countdownTime;
        StartCountdown();
        playerVariables = GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Game Countdown
        if (startCountdown) {
            currentTime -= 1 * Time.deltaTime;
            countdownText.text = currentTime.ToString("0");
            if (currentTime <= 3)
            {
                countdownOverlay.SetActive(true);
                countdownText.color = Color.red;
                countdownText.fontSize = 50;
                countdownText.transform.position = new Vector3(9f, 1f, 0);
            }
            if (currentTime <= 0)
            {
                currentTime = 0;
                startCountdown = false;
                countdownOverlay.SetActive(false);
                countdownUnderline.SetActive(false);
                countdownText.enabled = false;
            }
        }

        //Health Damage Test
        if (Input.GetKeyDown(KeyCode.L))
        {
            playerVariables.health -= damageTaken;
            healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(playerVariables.health, 90);
        }

        //Debug.Log("fire" + playerVariables.health);

        if (healthShadow > health)
        {
            healthBarShadow.GetComponent<RectTransform>().sizeDelta = new Vector2(healthShadow, 90);
            healthShadow -= 100 * Time.deltaTime;
        }

        if (health <= 0)
        {
            isDead = true;
        }
    }

    private void Reset()
    {
        //Countdown
        currentTime = 10f;
    }

    public void StartCountdown()
    {
        Debug.Log(currentTime);

        startCountdown = true;

    }

}


