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

    //References
    public GameObject healthUI;
    private HealthUI healthUIScript;

    public GameObject playerManager;
    private PlayerManager playerManagerScript;

    public GameObject cooldowns;
    private CooldownScript cooldownScript;

    //Floats and Bools
    float currentTime;
    bool onoff;
    bool abilityActivationState;
    bool cooldownActivationState;
    float originalAnimationScale;

    //Color color = Color.red;

    [Header("Animation Variables")]
    [SerializeField]
    private Vector2 transformPosition = Vector2.zero;

    [Range(1.0f, 10.0f), SerializeField]
    private float animationDuration = 3f;

    [Range(1.0f, 2.0f), SerializeField]
    private float animationScale = 1.75f;

    [SerializeField]
    //private Ease animationType = Ease.Linear;

    void Start()
    {
        //Setting References
        healthUIScript = healthUI.GetComponent<HealthUI>();
        cooldownScript = cooldowns.GetComponent<CooldownScript>();

        playerManagerScript = playerManager.GetComponent<PlayerManager>();

        originalAnimationScale = deathTimer.GetComponent<RectTransform>().localScale.x;

        //Setting Default Spawn Location and Death Timer
        currentTime = 5f;
        spawnLocation = new Vector3(0, 0, 0);
    }

    void Update()
    {
        //Activates When The Player is Dead
        if (playerManagerScript.isDead) {

            //Activates Death Overlay
            onoff = true;
            DeathScreen(onoff);
            deathText.DOColor(Color.red, animationDuration);

            //Stars Death Timer
            currentTime -= 1 * Time.deltaTime;

            //Activates Death Overlay UI
            deathTimer.text = currentTime.ToString("0");
            deathText.transform.DOScale(animationScale, animationDuration);

            //Deactiviating ability cooldowns
            abilityActivationState = true;
            cooldownActivationState = false;
            AbilityActivation(abilityActivationState, cooldownActivationState);
            
        }
         
        if(currentTime <= 0)
        {
            //Reset Health
            HealthReset();

            //Reset Death Text and Color to Default Size and Color
            deathText.transform.DOScale(originalAnimationScale, animationDuration);
            deathText.DOColor(Color.white, animationDuration);


            //Deactiviating ability cooldowns
            abilityActivationState = false;
            AbilityActivation(abilityActivationState, cooldownActivationState);

            //Resets Death Timer to Default Count
            currentTime = 5f;

            //Deactivate Death Overlay
            onoff = false;
            DeathScreen(onoff);

            //Respawns Player
            SpawnPlayer();
        }

    }

    //Activates Death Overlay
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

    //Spawn Locations
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

    //Resets Health After Respawn
    private void HealthReset()
    {
        playerManagerScript.isDead = false;
        playerManagerScript.health = 100; //Subject to change
        healthUIScript.healthBarUI = 680;
        healthUIScript.healthShadow = healthUIScript.healthBarUI;
        healthUIScript.healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(healthUIScript.healthBarUI, 90);
        healthUIScript.healthBarShadow.GetComponent<RectTransform>().sizeDelta = new Vector2(healthUIScript.healthShadow, 90);
    }

    //Resets Ability Cooldowns After Respawn
    private void AbilityActivation(bool abilityActivationState, bool cooldownActivationState)
    {
        //Resets Ability Active
        cooldownScript.cooldownActiveC = abilityActivationState;
        cooldownScript.cooldownActiveX = abilityActivationState;
        cooldownScript.cooldownActiveQ = abilityActivationState;
        cooldownScript.cooldownActiveE = abilityActivationState;

        //Resets UI Cooldown Bar
        cooldownScript.heightC = 100f;
        cooldownScript.heightX = 100f;
        cooldownScript.heightQ = 100f;
        cooldownScript.heightE = 100f;

        //Resets UI Active
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
