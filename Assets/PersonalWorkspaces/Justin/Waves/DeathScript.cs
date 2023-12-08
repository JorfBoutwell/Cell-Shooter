using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DeathScript : MonoBehaviour
{
    [Header("Player Team/Spawn")]
    public Vector3 spawnLocation;
    public GameObject player;
    public string playerTeam;

    [Header("Death UI")]
    public GameObject deathOverlay;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI deathTimer;

    public GameObject waveStart;
    private WaveStart waveStartScript;

    float currentTime;
    bool deathCondition = false;
    bool onoff;
    Color color = Color.red;

    [Header("Animation Variables")]
    [SerializeField]
    private Vector2 transformPosition = Vector2.zero;

    [Range(1.0f, 10.0f), SerializeField]
    private float animationDuration = 3f;

    [Range(1.0f, 2.0f), SerializeField]
    private float animationScale = 1.75f;

    [SerializeField]
    private Ease animationType = Ease.Linear;

    // Start is called before the first frame update
    void Start()
    {
        //Player Team
        playerTeam = "A";

        //CHANGE AFTER DEATH WORKS
        deathCondition = true;

        waveStartScript = waveStart.GetComponent<WaveStart>();

        currentTime = 5f;
        spawnLocation = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (waveStartScript.isDead) { //or deathCondition for testing
            onoff = true;
            DeathScreen(onoff);
            //deathText.DOColor(color, animationDuration);
            currentTime -= 1 * Time.deltaTime;
            deathTimer.text = currentTime.ToString("0");
            deathText.transform.DOScale(animationScale, animationDuration);
        }
         
        if(currentTime <= 0)
        {
            deathCondition = false;

            waveStartScript.isDead = false;
            waveStartScript.health = 680;
            waveStartScript.healthShadow = 680;

            currentTime = 5f;
            onoff = false;
            DeathScreen(onoff);
            SpawnPlayer();
        }

    }

    private void DeathScreen(bool onoff)
    {
        if (onoff) { 
        deathOverlay.SetActive(true);
        }
        else
        {
        deathOverlay.SetActive(false);
        }
        return;
    }

    private void SpawnPlayer()
    {
        if (playerTeam == "A") {
            
            spawnLocation = GameObject.FindGameObjectWithTag("SpawnA").transform.position;
            player.transform.position = spawnLocation;
        }
        else if (playerTeam == "B")
        {
            spawnLocation = GameObject.FindGameObjectWithTag("SpawnB").transform.position;
            player.transform.position = spawnLocation;
        }
    }
}
