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
        pointUpdateScript = gameObject.transform.root.GetComponentInChildren<PointUpdateScript>();
        currentTime = countdownTime;
        StartCountdown();
        countdownTimer = countdownText.GetComponentInChildren<TextMeshProUGUI>();
        PlayerManager playerManagerScript = GetComponentInParent<PlayerManager>();

        objectiveText.SetActive(false);
        objectiveTextLine.SetActive(false);

        //assign Points A and B
        pointsA = GameObject.Find("PointsA");
        pointsB = GameObject.Find("PointsB");

        //Objective Text Prompts
        objectiveTextPrompts.Add("Your team needs to reach 1000 points to win!");
        objectiveTextPrompts.Add("Earn points by claiming buttons around the map!");
        objectiveTextPrompts.Add("Simply touch a button to claim it!");
        objectiveTextPrompts.Add("Enemy players can claim your buttons instantly!");
        objectiveTextPrompts.Add("If you die, all your buttons will be unclaimed!");
        objectiveTextPrompts.Add("You can't wall jump forever!");

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
                countdownTimer.fontSize = 200;
                countdownText.transform.position = new Vector3(1000f, 200f, 0f);
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
                    countdownTimer.fontSize = 50;
                    transform.root.gameObject.GetComponent<PhotonView>().RPC("endGame", RpcTarget.AllViaServer);
                }

                if (!gameTimerStart) { 
                startClock(); //J JERE
                }
            }

            if(pointsA.GetComponent<PointsADisplayScript>().points >= 1000 && !win)
            {
                win = true;
                winTeam = "A";
                countdownTimer.text = "GAME OVER";
                countdownTimer.fontSize = 50;
                transform.root.gameObject.GetComponent<PhotonView>().RPC("endGame", RpcTarget.AllViaServer);
            }
            else if(pointsB.GetComponent<PointsADisplayScript>().points >= 1000 && !win)
            {
                win = true;
                winTeam = "B";
                countdownTimer.text = "GAME OVER";
                countdownTimer.fontSize = 50;
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
        countdownTimer.fontSize = 55f;
        countdownText.transform.position = new Vector3(958f, 540f, 0f);
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

            if (pointsA.GetComponent<PointsADisplayScript>().points > pointsB.GetComponent<PointsADisplayScript>().points || winTeam == "A")
            {
                winText.GetComponentInChildren<TextMeshProUGUI>().text = "Team A Wins!";
                Debug.Log("Team A Wins!");
            }
            else if (pointsB.GetComponent<PointsADisplayScript>().points > pointsA.GetComponent<PointsADisplayScript>().points || winTeam == "B")
            {
                winText.GetComponentInChildren<TextMeshProUGUI>().text = "Team B Wins!";
                Debug.Log("Team B Wins!");
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
        Debug.Log("yeep");
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
        objectiveText.GetComponentInChildren<TextMeshProUGUI>().DOFade(0, 1);

        objectiveText.SetActive(true);
        objectiveTextLine.SetActive(true);

        for (int i = 0; i < objectiveTextPrompts.Count; i++)
            {
            objectiveText.GetComponentInChildren<TextMeshProUGUI>().text = objectiveTextPrompts[i];
            objectiveText.GetComponentInChildren<TextMeshProUGUI>().DOFade(1, 1);

            yield return new WaitForSeconds(2.75f);

            objectiveText.GetComponentInChildren<TextMeshProUGUI>().DOFade(0, 1);

            if(i == objectiveTextPrompts.Count - 1)
            {
                yield return new WaitForSeconds(1f);
                objectiveText.SetActive(false);
                objectiveTextLine.SetActive(false);
            }

            yield return new WaitForSeconds(2.75f);

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


