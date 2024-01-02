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

    public GameObject playerManager;
    private PlayerManager playerManagerScript;

    public GameObject cooldowns;
    private CooldownScript cooldownScript;

    float currentTime;
    bool deathCondition = false;
    bool onoff;
    bool abilityActivationState;
    bool cooldownActivationState;
    float originalAnimationScale;
    Color color = Color.red;

    [Header("Animation Variables")]
    [SerializeField]
    private Vector2 transformPosition = Vector2.zero;

    [Range(1.0f, 10.0f), SerializeField]
    private float animationDuration = 3f;

    [Range(1.0f, 2.0f), SerializeField]
    private float animationScale = 1.75f;

    [SerializeField]
   // private Ease animationType = Ease.Linear;

    // Start is called before the first frame update
    void Start()
    {
        //Player Team
        

        //CHANGE AFTER DEATH WORKS
        deathCondition = true;

        waveStartScript = waveStart.GetComponent<WaveStart>();
        cooldownScript = cooldowns.GetComponent<CooldownScript>();

        playerManagerScript = playerManager.GetComponent<PlayerManager>();

        originalAnimationScale = deathTimer.GetComponent<RectTransform>().localScale.x;
        currentTime = 5f;
        spawnLocation = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerManagerScript.isDead) { //or deathCondition for testing
            onoff = true;
            DeathScreen(onoff);
            //deathText.DOColor(color, animationDuration);
            currentTime -= 1 * Time.deltaTime;
            deathTimer.text = currentTime.ToString("0");
            deathText.transform.DOScale(animationScale, animationDuration);

            //Deactiviating ability cooldowns
            abilityActivationState = true;
            cooldownActivationState = false;
            AbilityActivation(abilityActivationState, cooldownActivationState);
            
        }
         
        if(currentTime <= 0)
        {
            deathCondition = false;

            HealthReset();

         //   deathText.transform.DOScale(originalAnimationScale, animationDuration);

            //Deactiviating ability cooldowns
            abilityActivationState = false;
            AbilityActivation(abilityActivationState, cooldownActivationState);

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
        if (playerManagerScript.team == "A") {
            
            spawnLocation = GameObject.FindGameObjectWithTag("SpawnA").transform.position;
            player.transform.position = spawnLocation;
        }
        else if (playerManagerScript.team == "B")
        {
            spawnLocation = GameObject.FindGameObjectWithTag("SpawnB").transform.position;
            player.transform.position = spawnLocation;
        }
    }

    private void HealthReset()
    {
        playerManagerScript.isDead = false;
        playerManagerScript.health = 680;
        waveStartScript.healthShadow = 680;
        waveStartScript.healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(playerManagerScript.health, 90);
        waveStartScript.healthBarShadow.GetComponent<RectTransform>().sizeDelta = new Vector2(waveStartScript.healthShadow, 90);
    }

    private void AbilityActivation(bool abilityActivationState, bool cooldownActivationState)
    {
        cooldownScript.cooldownActiveC = abilityActivationState;
        cooldownScript.cooldownActiveX = abilityActivationState;
        cooldownScript.cooldownActiveQ = abilityActivationState;
        cooldownScript.cooldownActiveE = abilityActivationState;

        cooldownScript.heightC = 100f;
        cooldownScript.heightX = 100f;
        cooldownScript.heightQ = 100f;
        cooldownScript.heightE = 100f;

        cooldownScript.cooldownOverlayC.SetActive(cooldownActivationState);
        cooldownScript.cooldownOverlayBarC.SetActive(cooldownActivationState);

        cooldownScript.cooldownOverlayX.SetActive(cooldownActivationState);
        cooldownScript.cooldownOverlayBarX.SetActive(cooldownActivationState);

        cooldownScript.cooldownOverlayQ.SetActive(cooldownActivationState);
        cooldownScript.cooldownOverlayBarQ.SetActive(cooldownActivationState);

        cooldownScript.cooldownOverlayE.SetActive(cooldownActivationState);
        cooldownScript.cooldownOverlayBarE.SetActive(cooldownActivationState);

    }
}
