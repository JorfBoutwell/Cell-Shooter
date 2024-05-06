using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public PointCollectorBar pointCollectorBarScript;

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
        //pointCollectorBarScript = GameObject.Find("PointCollectorBar").GetComponent<PointCollectorBar>();

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
        } else
        {
            initialDeathCode = false;
            currentTime = 5f;
        }
         
    }

    public IEnumerator Death()
    {
        
        if (initialDeathCode == false)
        {
            

            //Unclaims Points
            PointCollecterReset();
            Debug.Log((goober.GetComponent<GooberFunctionality>().gameObject.transform.parent.gameObject == gameObject) + "They match");
            if (goober != null && goober.GetComponent<GooberFunctionality>().currentPlayer == gameObject)
            {
                goober.GetComponent<GooberFunctionality>().atpClaimed = false;
                //drop goober
                Debug.Log("entered if");
                DropGoober();
            }
            DropGoober();
            goober.GetComponent<GooberFunctionality>().atpClaimed = false;

            //Activates Death Overlay
            onoff = true;
            DeathScreen(onoff);

            //Respawns Player
            SpawnPlayer();

            initialDeathCode = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }

        //Activates Death Overlay UI
        deathTimer.text = currentTime.ToString("0");

        yield return new WaitForSeconds(currentTime);

        foreach (GameObject a in player.GetComponent<WeaponManager>().abilityUI.abilityObjects)
        {
            a.transform.GetChild(3).gameObject.GetComponent<Image>().enabled = false;
            a.transform.GetChild(0).GetChild(1).GetComponent<Image>().enabled = true;
        }

        //Reset Health
        HealthReset();

        //Deactivate Death Overlay
        onoff = false;
        DeathScreen(onoff);

        //SpawnPlayer();

        //REACTIVATE GOOBER GUIDANCE SYSTEM
        gooberGuide.SetActive(true);

        //goober.GetComponent<SphereCollider>().enabled = true;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;

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
        Debug.Log("hee" + playerManagerScript.pointCollection.Length);
        //Does playerManagerScript.pointCollectors.Count get reset to 0 somewehre before this for loop
        //Okay, talk to Henry about storing pointcollectors when collided and reseting the ones from the player that died
        for (int i = 0; i < playerManagerScript.pointsCollectedIndexList.Count; i++)
        {
            playerManagerScript.pointCollection[playerManagerScript.pointsCollectedIndexList[i]].GetComponentInChildren<Renderer>().material.color = Color.grey;
            playerManagerScript.pointCollection[playerManagerScript.pointsCollectedIndexList[i]].GetComponentInChildren<PointCollectorScript>().currentPlayer = null;
            Debug.Log("Heep");
        }
        playerManagerScript.pointsCollectedIndexList.Clear();
        playerManagerScript.pointCollectors.Clear();

        //pointCollectorBarScript.updateBar(playerManagerScript.team, -1 * playerManagerScript.buttonsPressed);
        //playerManagerScript.buttonsPressed = 0;

    }

    public void DropGoober()
    {
        if (playerManagerScript.buttonsPressed != 1)
        {
            return;
        }
        Debug.Log("dropped Goober");
        //change ui of abiities and stuff
        foreach(GameObject a in goober.GetComponent<GooberFunctionality>().currentPlayer.GetComponent<WeaponManager>().abilityUI.abilityObjects)
        {
            a.transform.GetChild(3).gameObject.GetComponent<Image>().enabled = false;
        }
        //drop timer is set to 5.2, can't be picked up until after the timer because of a stupid way death occurs
        goober.GetComponent<GooberFunctionality>().dropped = 1f;
        //set the parent to null
        goober.transform.SetParent(null);
        //drop it
        // goober.transform.position += new Vector3(0, -1.5f, 0);
        //set new parent back to empty
        goober.transform.SetParent(GameObject.Find("Goobers").transform);
        //turn on the collider
        goober.GetComponent<SphereCollider>().enabled = true;
        //run kill feed
        killFeedScript.AlertFeedInstantiate(killFeedScript.boxesCount, (goober.GetComponent<GooberFunctionality>().currentPlayer.gameObject.GetComponent<PlayerManager>().username + " dropped the ATP!"));
        //set current player and team to null
        goober.GetComponent<GooberFunctionality>().currentPlayer = null;
        goober.GetComponent<GooberFunctionality>().team = null;
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

    private void AmmoReset()
    {
        WeaponManager w = player.GetComponent<WeaponManager>();
        w.currentAmmo = w.currentWeapon.maxAmmo;
    }

    //Resets Ability Cooldowns After Respawn
}
