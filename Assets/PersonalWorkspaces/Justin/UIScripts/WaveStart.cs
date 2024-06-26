using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class WaveStart : MonoBehaviourPunCallbacks
{
    //Start variables
    static public bool playersReady;
    static public bool enemySpawnActive;
    static public bool pickUpSpawn;
    public bool gameStart;

    //Countdown variables
    public GameObject countdownText;
    public TextMeshProUGUI countdownTimer;
    public GameObject countdownUnderline;
    public GameObject countdownOverlay;
    public float currentTime;
    public float countdownTime = 10f;
    public float gameTimeSeconds;
    public float gameTimeMinutes;
    public bool startCountdown = false;
    public bool gameTimerStart = false;
    public Vector3 timerPosition;

    //bool to determine if que has been loaded
    public bool queLoad = false;

    //Objective variables
    public List<string> objectiveTextPrompts = new List<string>();
    public GameObject objectiveText;
    public GameObject objectiveTextLine;

    //End Variables
    public PointUpdateScript pointUpdateScript;
    public GameObject winOverlay;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI returnTimer;
    public float returnTime = 5f;
    public string winTeam;
    public int pointsNeeded = 1000;

    //PointUIGameObject
    public GameObject pointsA;
    public GameObject pointsB;

    public GameObject dictionary;

    public bool win = false;

    //custom variable for master client ready setup
    private static readonly string TeamPropKey = "startGame";
    private bool start = false;

    void Start()
    {

        if (GameObject.Find("Goober"))
        {
            pointsNeeded = 100;
        } 
        pointUpdateScript = gameObject.transform.root.GetComponentInChildren<PointUpdateScript>();
        currentTime = countdownTime;
        StartCountdown();
        countdownTimer = countdownText.GetComponentInChildren<TextMeshProUGUI>();
        PlayerManager playerManagerScript = GetComponentInParent<PlayerManager>();

        objectiveText.SetActive(false);
        objectiveTextLine.SetActive(false);

        //Objective Text Prompts
        objectiveTextPrompts.Add("Use keys WASD to move and spacebar to jump");
        objectiveTextPrompts.Add("Press down left click to shoot");
        objectiveTextPrompts.Add("Jump on walls to move around the map");
        objectiveTextPrompts.Add("Watch for trains!");
        objectiveTextPrompts.Add("Follow the arrow to find the ATP");
        objectiveTextPrompts.Add("Claim the ATP to recieve points for your team");
        objectiveTextPrompts.Add("You can't shoot or use your abilities with the ATP");
        objectiveTextPrompts.Add("If you die, you'll lose the ATP!");
        objectiveTextPrompts.Add("Your team needs to reach " + pointsNeeded + " points to win!");
        
        
        

        //Finding WinCondition Objects
        winOverlay = GameObject.Find("WinOverlay");
        winText = GameObject.Find("WinText").GetComponentInChildren<TextMeshProUGUI>();
        returnTimer = GameObject.Find("ReturnTimer").GetComponentInChildren<TextMeshProUGUI>();
        winOverlay.SetActive(false);

        

    }

    void Update()
    {

        //Game Countdown
        if (startCountdown)
        {
            if (!win)
            {
                currentTime -= 1 * Time.deltaTime;
                countdownTimer.text = currentTime.ToString("0");
            } 

            if(gameTimerStart && !win)
            {
                gameTimeMinutes = Mathf.FloorToInt(currentTime / 60);
                gameTimeSeconds = Mathf.FloorToInt(currentTime % 60);
                countdownTimer.text = string.Format("{0:00}:{1:00}", gameTimeMinutes, gameTimeSeconds);
            }

            //Activates Final Countdown Overlay and Changes
            if (currentTime <= 3 && !gameTimerStart)
            {
                countdownOverlay.SetActive(true);
                countdownTimer.color = Color.red;
            }

            //Deactivates Final Countdown Overlay and Countdown
            if (currentTime <= 0)
            {
                if (gameTimerStart == false && PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { TeamPropKey, true } });
                }
                else if (gameTimerStart)
                {
                    countdownTimer.text = "GAME OVER";
                    countdownTimer.fontSize = 45;
                    transform.root.gameObject.GetComponent<PhotonView>().RPC("endGame", RpcTarget.AllViaServer);
                }

                if (!gameTimerStart) { 
                startClock(); //J JERE
                }
            }
            if(pointsA.GetComponentInChildren<PointsADisplayScript>().points >= pointsNeeded && !win)
            {
                win = true;
                winTeam = "A";
                countdownTimer.text = "GAME OVER";
                countdownTimer.fontSize = 45;
                transform.root.gameObject.GetComponent<PhotonView>().RPC("endGame", RpcTarget.AllViaServer);
            }
            else if(pointsB.GetComponentInChildren<PointsADisplayScript>().points >= pointsNeeded && !win)
            {
                win = true;
                winTeam = "B";
                countdownTimer.text = "GAME OVER";
                transform.root.gameObject.GetComponent<PhotonView>().RPC("endGame", RpcTarget.AllViaServer);
            }

            if(win)
            {
                returnTimer.text = returnTime.ToString("0");
                returnTime -= 1 * Time.deltaTime;
            }
        }
    }

    //Resets Countdown to Start Game Timer
    public void Reset()
    {
        currentTime = 180f;
        gameTimerStart = true;

        StartCountdown();
        countdownTimer.color = Color.white;
    }

    //Starts Countdown/Game Timer
    public void StartCountdown()
    {
        startCountdown = true;
        StartCoroutine("ObjectiveEnter");

    }

    public void WinCondition(string winTeam)
    {
        
        //GetComponent<PlayerManager>().inputActions.Disable();

            if (pointsA.GetComponentInChildren<PointsADisplayScript>().points > pointsB.GetComponentInChildren<PointsADisplayScript>().points || winTeam == "A")
            {
                winText.GetComponentInChildren<TextMeshProUGUI>().text = "Team A Wins!";
            }
            else if (pointsB.GetComponentInChildren<PointsADisplayScript>().points > pointsA.GetComponentInChildren<PointsADisplayScript>().points || winTeam == "B")
            {
                winText.GetComponentInChildren<TextMeshProUGUI>().text = "Team B Wins!";
            }

            winOverlay.SetActive(true);
            winText.transform.DOScale(1.75f, 3);
            winText.DOColor(Color.yellow, 3);

            //returnTimer.text = returnTime.ToString("0");
            //returnTime -= 1 * Time.deltaTime;

            
            StartCoroutine("EnterQueueScene");
        
        
    }

    public void startClock()
    {
        gameObject.transform.parent.GetComponentInChildren<PointUpdateScript>().time = 0;
        currentTime = 0f;
        countdownOverlay.SetActive(false);
        Reset();
        

    }

    //runs every time a property is updated
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        //if the change being made is for the local user
        if (targetPlayer != null && targetPlayer.IsMasterClient && gameTimerStart)
        {
            startClock();
        }
    }

    //Displays Objective Text
    IEnumerator ObjectiveEnter()
    {
        
            //GameObject.Find("CaptureTheFlagOverlay").SetActive(false);
            objectiveText.GetComponentInChildren<TextMeshProUGUI>().DOFade(0, 1);

            objectiveText.SetActive(true);
            objectiveTextLine.SetActive(true);

            for (int i = 0; i < objectiveTextPrompts.Count; i++)
            {
                objectiveText.GetComponentInChildren<TextMeshProUGUI>().text = objectiveTextPrompts[i];
                objectiveText.GetComponentInChildren<TextMeshProUGUI>().DOFade(1, 1);

                yield return new WaitForSeconds(2.75f);

                objectiveText.GetComponentInChildren<TextMeshProUGUI>().DOFade(0, 1);

                if (i == objectiveTextPrompts.Count - 1)
                {
                    yield return new WaitForSeconds(1f);
                    objectiveText.SetActive(false);
                    objectiveTextLine.SetActive(false);
                }

                yield return new WaitForSeconds(1.75f);

            }
        
        
    }

    IEnumerator EnterQueueScene()
    {
        Debug.Log("made it here");
        PhotonNetwork.AutomaticallySyncScene = true;
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        dictionary = GameObject.Find("CustomVariableStorage");
        Destroy(dictionary);

        yield return new WaitForSeconds(5f);
        if (PhotonNetwork.IsMasterClient)
        {
            LoadQueue();
            //transform.root.gameObject.GetComponent<PhotonView>().RPC("loadLevel", RpcTarget.AllBufferedViaServer);
        }
    }

    public void LoadQueue()
    {
        if (!queLoad)
        {
            PhotonNetwork.LoadLevel("Queue");
            queLoad = true;
        }
    }

}


