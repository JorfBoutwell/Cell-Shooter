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

    public PlayerManager playerManagerScript;

    public GameObject cooldowns;
    private CooldownScript cooldownScript;

    public GameObject pointCollector;
    public PointCollectorScript pointCollectorScript;

    //Floats and Bools
    float currentTime;
    bool onoff;
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

        originalAnimationScale = deathTimer.GetComponent<RectTransform>().localScale.x;

        //Setting Default Spawn Location and Death Timer
        currentTime = 5f;
        spawnLocation = new Vector3(0, 0, 0);
    }

    void Update()
    {

        //Activates When The Player is Dead
        if (playerManagerScript.isDead) {
            StartCoroutine(Death());
        }
         
    }

    public IEnumerator Death()
    {
        //Unclaims Points
        PointCollecterReset();

        //Activates Death Overlay
        onoff = true;
        DeathScreen(onoff);
        deathText.DOColor(Color.red, animationDuration);

        //Activates Death Overlay UI
        deathTimer.text = currentTime.ToString("0");
        deathText.transform.DOScale(animationScale, animationDuration);

        yield return new WaitForSeconds(currentTime);

        //Reset Health
        HealthReset();

        //Reset Death Text and Color to Default Size and Color
        deathText.transform.DOScale(originalAnimationScale, animationDuration);
        deathText.DOColor(Color.white, animationDuration);

        //Deactivate Death Overlay
        onoff = false;
        DeathScreen(onoff);

        //Respawns Player
        SpawnPlayer();

        //Resets Death Timer to Default Count
        currentTime = 5f;
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
            
            player.transform.position = playerManagerScript.spawn;
        }
        else if (playerManagerScript.team == "B")
        {
            player.transform.position = playerManagerScript.spawn;
        }
    }

    public void PointCollecterReset()
    {
        Debug.Log("hee");
        for(int i = 0; i < playerManagerScript.pointCollectors.Count; i++)
        {
            playerManagerScript.pointCollectors[i].GetComponentInChildren<Renderer>().material.color = Color.grey;
            playerManagerScript.pointCollectors[i].GetComponentInChildren<PointCollectorScript>().currentPlayer = null;
            Debug.Log("Heep");
        }

        playerManagerScript.pointCollectors.Clear();

        /*if (playerManagerScript.team == "A")
           {
            playerManagerScript.currentPointCollectorsA = 0;
           }
        else if(playerManagerScript.team == "B")
        {
            playerManagerScript.currentPointCollectorsB = 0;
        }*/

        playerManagerScript.buttonsPressed = 0;
    }

    //Resets Health After Respawn
    private void HealthReset()
    {
        playerManagerScript.isDead = false;

        playerManagerScript.health = 100; //Subject to change

        healthUIScript.healthBarUI = playerManagerScript.health * 6.8f;
        Debug.Log("e" + healthUIScript.healthBarUI);
        healthUIScript.healthShadow = healthUIScript.healthBarUI;
        Debug.Log("r" + healthUIScript.healthShadow);
        healthUIScript.healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(healthUIScript.healthBarUI, 90);
        Debug.Log("q" + healthUIScript.healthBarUI);
        healthUIScript.healthBarShadow.GetComponent<RectTransform>().sizeDelta = new Vector2(healthUIScript.healthShadow, 90);
    }

    //Resets Ability Cooldowns After Respawn
}
