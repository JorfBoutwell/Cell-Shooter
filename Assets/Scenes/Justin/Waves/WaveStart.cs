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

    public GameObject cooldownOverlayC;
    public GameObject cooldownOverlayBarC;

    public TextMeshPro bulletCount;

    public float currentTime = 0f;
    public float countdownTime = 10f;

    public bool startCountdown = false;
    public bool gameStart;

    public bool cooldownActiveC = false;
    float newHeight = 100;

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

        if(cooldownActiveC == false)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                CooldownActive();
                Debug.Log("hi");
                cooldownActiveC = true;
                
            }
        }

        if (newHeight > 0 && cooldownActiveC)
        {
            Debug.Log("yo");
            //cooldownOverlayBarC.GetComponent<RectTransform>().transform.localScale = ();
            cooldownOverlayBarC.GetComponent<RectTransform>().sizeDelta = new Vector2(100, newHeight);
            newHeight -= 10 * Time.deltaTime;
        }
        else if (newHeight <= 0)
        {
            cooldownActiveC = false;
        }

    }

    private void Reset()
    {
        //Countdown
        currentTime = 10f;
        //EnemyDespawn
    }

    private void CooldownActive()
    {
        cooldownOverlayC.SetActive(true);
        cooldownOverlayBarC.SetActive(true);

        //StartCoroutine("");
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
        
    }
}


