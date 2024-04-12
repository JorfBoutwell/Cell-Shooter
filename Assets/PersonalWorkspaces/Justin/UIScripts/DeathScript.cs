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
    public TextMeshProUGUI deathTimer;
    public TextMeshProUGUI deathMessage;
    public GameObject gooberGuide;
    public List<string> messages;
    List<string> tempMessages;
    public float fadeInDuration;
    public float fadeOutDuration;

    //References
    public GameObject healthUI;
    private HealthUI healthUIScript;

    public PlayerManager playerManagerScript;

    public GameObject cooldowns;
    private CooldownScript cooldownScript;

    public GameObject pointCollector;
    public PointCollectorScript pointCollectorScript;

    public KillFeed killFeedScript;

    public GameObject goober;

    //Floats and Bools
    bool initialDeathCode = false;
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
        killFeedScript = GameObject.Find("KillFeed").GetComponent<KillFeed>();


        originalAnimationScale = deathTimer.GetComponent<RectTransform>().localScale.x;

        //Setting Default Spawn Location and Death Timer
        currentTime = 5f;
        spawnLocation = new Vector3(0, 0, 0);

        goober = GameObject.FindGameObjectWithTag("Goober");

        Random.InitState(System.DateTime.Now.Millisecond);

        tempMessages = new List<string>(messages);
    }

    void Update()
    {

        //Activates When The Player is Dead
        if (playerManagerScript.isDead) {
            currentTime -= Time.deltaTime;
            StartCoroutine(Death());
        }
         
    }

    public IEnumerator Death()
    {
        if (initialDeathCode == false)
        {
            //Unclaims Points
            PointCollecterReset();

            //drop goober
            DropGoober();

            //Activates Death Overlay
            onoff = true;
            DeathScreen(onoff);

            initialDeathCode = true;
        }

        //Activates Death Overlay UI
        deathTimer.text = currentTime.ToString("0");

        yield return new WaitForSeconds(currentTime);

        //Reset Health
        HealthReset();

        //Deactivate Death Overlay
        onoff = false;
        DeathScreen(onoff);

        //Respawns Player
        SpawnPlayer();

        //REACTIVATE GOOBER GUIDANCE SYSTEM
        gooberGuide.SetActive(true);

        //Resets Death Timer to Default Count
        currentTime = 5f;

        initialDeathCode = false;
    }

    //Activates Death Overlay
    private void DeathScreen(bool onoff)
    {
        CanvasGroup deathGroup = deathOverlay.GetComponent<CanvasGroup>();
        deathGroup.alpha = 0f;
        
        if (onoff) {


            if(tempMessages.Count <= 0)
            {
                tempMessages = new List<string>(messages);
            }
            else
            {
                int r = Random.Range(0, tempMessages.Count - 1);
                deathMessage.text = tempMessages[r];
                tempMessages.Remove(tempMessages[r]);
            }

            deathOverlay.SetActive(true);
            deathGroup.DOFade(1f, fadeInDuration);
        }
        else
        {
            deathGroup.DOFade(0f, fadeOutDuration);
        //deathOverlay.SetActive(false);
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

    public void DropGoober()
    {
        goober.GetComponent<GooberFunctionality>().dropped = 5.01f;
        goober.transform.SetParent(null);
        goober.transform.position += new Vector3(0, -1.5f, 0);
        goober.transform.SetParent(GameObject.Find("Goobers").transform);
        goober.GetComponent<SphereCollider>().enabled = true;
        killFeedScript.AlertFeedInstantiate(killFeedScript.boxesCount, (goober.GetComponent<GooberFunctionality>().currentPlayer.gameObject.GetComponent<PlayerManager>().username + " dropped the ATP!"));
        goober.GetComponent<GooberFunctionality>().currentPlayer = null;
        goober.GetComponent<GooberFunctionality>().team = null;
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
