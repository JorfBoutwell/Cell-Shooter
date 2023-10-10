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
    public GameObject countdownOverlay;
    public float currentTime = 0f;
    public float countdownTime = 10f;
    public bool startCountdown = false;

    //Bullet variables
    public TextMeshPro bulletCount;
    float bulletAmount = 100f;

    //Healthbar variables
    public GameObject healthBar;
    public GameObject healthBarShadow;
    float health = 680;
    float healthShadow = 680;

    float damageTaken = 100f;

    //Death variables
    public bool isDead = false;

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
                countdownText.enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            health -= damageTaken;
            healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(health, 90);
            Debug.Log("hey" + health);

        }

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
        //EnemyDespawn
    }

    private void StartEnemySpawn()
    {
        if (enemySpawnActive)
        {

        }
    }

    private void StartPickUpSpawn()
    {
        if (pickUpSpawn)
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

    }

    

    public void Shoot()
    {
        if(Input.GetMouseButton(0))
        {
            Debug.Log("hola");
            bulletAmount -= 1f;
            bulletCount.text = bulletAmount.ToString("0");
        }

        if(Input.GetMouseButtonDown(0) && bulletAmount > 0)
        {
            Debug.Log("hola");
            bulletAmount -= 1f;
            bulletCount.text = bulletAmount.ToString("0");
        }
    }
}


