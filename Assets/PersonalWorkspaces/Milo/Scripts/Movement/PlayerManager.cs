//Milo Reynolds

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    PlayerControllerNEW m_player;
    WeaponManager m_weapon;

    [SerializeField] Transform playerCamera;

    [SerializeField] GameObject UI;
    [SerializeField] GameObject hitbox;

    [Header("UI References")]
    [SerializeField] GameObject damInd;
    [SerializeField] Image vignette;
    [SerializeField] GameObject FPDisplay;
    [SerializeField] Image charIcon;
    [SerializeField] Image teamInd;

    public InputActions inputActions;
    public PauseMenu pauseMenuScript;
    public Spawn spawnScript;
    public PointCollectorBar pointCollectorBarScript;
    public List<int> pointsCollectedIndexList = new List<int>();

    public PlayerRef[] playerRefs;
    public Stats[] stats;

    static int spawnIncrementA = 0;
    static int spawnIncrementB = 0;
    public Vector3 spawn;

    //variables I want to synch
    public float synch = 0f;
    public GameObject pointA;
    public GameObject pointB;

    public float maxHealth = 100;
    public float health = 100;
    public int ammo;
    public int stamina; // not implemented yet
    public int style; //not implemented yet
    public string team;
    public string character;
    public bool isDead = false;
    public bool deathTimerOn = false;

    private static readonly string TeamPropKey = "TeamA?";
    public string updatePoints;
    public int buttonsPressed;
    public List<GameObject> pointCollectors = new List<GameObject>();
    public int currentPointCollectorsA = 0;
    public List<string> activeEffects;
    public GameObject[] pointCollection;

    public GameObject goober;
    public GameObject gooberStatusOverlay;
    public GameObject gooberTargeter;

    PhotonView view;

    //keys for teamA and teamB scores
    private static readonly string TeamAScore = "TeamAScore";
    private static readonly string TeamBScore = "TeamBScore";

    private static readonly string IndividualCharacter = "individualCharacter";


    [SerializeField] Material materialA;
    [SerializeField] Material materialB;
    [SerializeField] Material materialDead;

    KillFeed killFeedScript;
    public string username;


    private void Awake()
    {
        StartCoroutine(SetFullscreen(0f, false));
        StartCoroutine(SetFullscreen(1f, true));
        spawnScript = GameObject.Find("SpawnPlayers").GetComponent<Spawn>();
        killFeedScript = GameObject.Find("KillFeed").GetComponent<KillFeed>();
        //pointCollectorBarScript = GameObject.Find("PointCollectorBar").GetComponent<PointCollectorBar>();

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { TeamAScore, 0 } });
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { TeamBScore, 0 } });
        }
        //username = PhotonNetwork.PlayerList[PhotonNetwork.PlayerList.Length - 1].ToString();
        //username = PhotonNetwork.LocalPlayer.NickName;
        //could try targetPlayer.NickName or PhotonNetwork.LocalPlayer instead

        character = "Neuron";

        inputActions = new InputActions();
        m_player = GetComponent<PlayerControllerNEW>();
        m_weapon = GetComponent<WeaponManager>();

        ammo = m_weapon.currentWeapon.maxAmmo;

        AssignInputs();

        //if this isn't the users it will destroy to avoid managing the other player
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(m_player.m_rb);
            Canvas[] canvases = GetComponentsInChildren<Canvas>();
            foreach (Canvas canvas in canvases)
            {
                Destroy(canvas);
            }


        }
        pointCollection = GameObject.FindGameObjectsWithTag("PointCollector");
        pointCollection = orderGoobers(pointCollection);

        goober = GameObject.FindGameObjectWithTag("Goober");
        gooberStatusOverlay = GameObject.Find("CaptureTheFlagOverlay");
        gooberTargeter = GameObject.Find("GooberDirection");

        pointA = GameObject.Find("PointA");
        pointB = GameObject.Find("PointB");
    }

    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (photonView.Owner.ActorNumber == player.ActorNumber)
            {
                isDead = false;
                username = player.NickName;
                object teamA;
                player.CustomProperties.TryGetValue(TeamPropKey, out teamA);
                object localCharacter;
                player.CustomProperties.TryGetValue(IndividualCharacter, out localCharacter);
                character = (string)localCharacter;
                

                if ((bool)teamA)
                {
                    team = "A";
                    transform.GetChild(0).gameObject.layer = 11;
                    gameObject.layer = 11;
                    gameObject.GetComponent<MeshRenderer>().material = materialA;
                    teamInd.color = Color.blue;

                    Debug.Log("spawnteama");
                    spawn = spawnScript.spawnPointsA[spawnScript.spawnIncrementA].transform.position;
                    this.gameObject.transform.position = spawn;
                    spawnScript.spawnIncrementA++;
                }
                else
                {
                    team = "B";
                    transform.GetChild(0).gameObject.layer = 13;
                    gameObject.layer = 13;
                    gameObject.GetComponent<MeshRenderer>().material = materialB;
                    teamInd.color = Color.red;


                    Debug.Log("spawnteamb");
                    spawn = spawnScript.spawnPointsB[spawnScript.spawnIncrementB].transform.position;
                    this.gameObject.transform.position = spawn;
                    spawnScript.spawnIncrementB++;
                }

            }
        }

        SetCharacter();
    }

    public void SetCharacter()
    {

        PlayerRef refs = playerRefs[0];
        switch(character)
        {
            case "Player1":
                Debug.Log("Neuron");
                refs = playerRefs[0];
                gameObject.AddComponent<NeuronStats>();
                break;
            case "Player2":
                Debug.Log("RBC");
                refs = playerRefs[1];
                gameObject.AddComponent<RBCStats>();
                break;
            case "Player3":
                Debug.Log("Osteoclast");
                refs = playerRefs[2];

                break;
            case "PLayer4":
                Debug.Log("TCell");
                refs = playerRefs[3];

                break;
            default: Debug.Log("Not a character " + character); break;
        }

        FPDisplay.GetComponent<Animator>().runtimeAnimatorController = refs.animator;
        m_weapon.abilityList = refs.abilities.ToArray();
        m_weapon.bulletTrail = refs.bulletTrail;
        charIcon.sprite = refs.icon;
        //m_weapon.weapon = refs.weapon;

        m_weapon.abilityUI.GenerateAbilityUI();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        if (stream.IsWriting)
        {
            stream.SendNext(rigidbody.position);
            stream.SendNext(rigidbody.rotation);
            stream.SendNext(rigidbody.velocity);
        }
        else
        {
            rigidbody.position = (Vector3)stream.ReceiveNext();
            rigidbody.rotation = (Quaternion)stream.ReceiveNext();
            rigidbody.velocity = (Vector3)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            rigidbody.position += rigidbody.velocity * lag;
        }
    }

    private void Update()
    { 
        if (!view.IsMine)
        {
            return;
        }
        if (ammo != m_weapon.currentAmmo)
        {
            ammo = m_weapon.currentAmmo;
        }

        if (isDead && !deathTimerOn)
        {
            deathTimerOn = true;
            StartCoroutine("DeathTimer");
        }

        if (isDead)
        {
            gameObject.GetComponent<MeshRenderer>().material = materialDead;
        } else if (team == "A")
        {
            gameObject.GetComponent<MeshRenderer>().material = materialA;
        } else if (team == "B")
        {
            gameObject.GetComponent<MeshRenderer>().material = materialB;
        }

        ChangeGooberText();

        synch += Time.deltaTime;
        if (synch > 2f && PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            synch = 0;
            photonView.RPC("synchPoints", RpcTarget.All, pointA.GetComponent<PointContainer>().points, pointB.GetComponent<PointContainer>().points);
        }
    }

    private GameObject[] orderGoobers(GameObject[] goobers)
    {
        string[] names = new string[goobers.Length];
        GameObject[] newGoober = new GameObject[goobers.Length];
        int i = 0;
        foreach (GameObject goober in goobers)
        {
            names[i] = goober.transform.parent.name;
            i++;
        }
        Array.Sort(names);
        for (int j = 0; j < names.Length; j++)
        {
            foreach (GameObject goober in goobers)
            {
                if (names[j] == goober.transform.parent.name)
                {
                    newGoober[j] = goober;
                    break;
                }
            }
        }
        return newGoober;
    }


    public void SetTeam(string teamName)
    {
        team = teamName;
    }


    private void AssignInputs()
    {
        inputActions.Movement.Jump.performed += ctx => m_player.Jump();
        inputActions.Movement.Sprint.performed += ctx => m_player.isSprinting = !m_player.isSprinting;
        inputActions.Movement.Sprint.canceled += ctx => m_player.isSprinting = !m_player.isSprinting;
        inputActions.Movement.Crouch.performed += ctx => m_player.isCrouching = !m_player.isCrouching;
        inputActions.Movement.Crouch.canceled += ctx => m_player.isCrouching = !m_player.isCrouching;

        inputActions.Weapon.Fire.started += ctx => m_weapon.FireWeapon();
        inputActions.Weapon.Fire.performed += ctx => m_weapon.isAutoFiring = true;
        inputActions.Weapon.Fire.canceled += ctx => m_weapon.isAutoFiring = false;

        inputActions.Weapon.Reload.performed += ctx => m_weapon.StartCoroutine(m_weapon.Reload());

        inputActions.Ability.Ability1.performed += ctx => m_weapon.UseAbility(0);
        inputActions.Ability.Ability2.performed += ctx => m_weapon.UseAbility(1);
        inputActions.Ability.Ability3.performed += ctx => m_weapon.UseAbility(2);
        inputActions.Weapon.Melee.performed += ctx => m_weapon.UseAbility(3);

        
        inputActions.Menu.PauseMenu.performed += ctx => pauseMenuScript.menuOnOff();
        
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void LeaveGame()
    {
        //clears your custom properties
        //ExitGames.Client.Photon.Hashtable customProperties = photonView.Owner.CustomProperties;
        //customProperties.Clear();
        //synchs the clearing
        //photonView.Owner.SetCustomProperties(customProperties);
        //leaves the room and loads the lobby
        GameObject dictionary = GameObject.Find("CustomVariableStorage");
        Destroy(dictionary);
        if(goober.GetComponent<GooberFunctionality>().currentPlayer == gameObject)
        {
            photonView.RPC("DropGoober", RpcTarget.All);
        }
        //leaves the room and loads the lobby
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        SceneManager.LoadSceneAsync("MainMenu");
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        
    }

    


    //ran when a point collecter hits this gameobject
    public void recievePoint(GameObject pointCollecter)
    {
        
        if (photonView.IsMine)
        {
            for(int i = 0; i < pointCollection.Length; i++)
            {
                Debug.Log(pointCollection.Length);
                if (pointCollection[i] == pointCollecter)
                {
                    photonView.RPC("startPointer", RpcTarget.All, i);
                    return;
                }
            }
            
        }
    }

    public void LoseAPoint(GameObject pointCollecter)
    {
        for (int i = 0; i < pointCollection.Length; i++)
        {
            Debug.Log(pointCollection.Length);
            if (pointCollection[i] == pointCollecter)
            {
                pointsCollectedIndexList.Remove(i);
            }
        }
    }

    //ran when you hit the goober
    public void CapturingTheFlag()
    {
        if(photonView.IsMine)
        {
            photonView.RPC("UpdateFlag", RpcTarget.All);
        }
    }

    
    public void ChangeGooberText()
    {
        GooberFunctionality gooberScript = goober.GetComponent<GooberFunctionality>();

        int target;
        if (gooberScript.currentPlayer == gameObject)
        {
            //turn on playerclaimed
            target = 1;
        } else if (gooberScript.team == "A")
        {
            //turn on either red
            target = 2;

            gooberTargeter.GetComponent<Image>().color = Color.red;
        } else if (gooberScript.team == "B")
        {
            //turn on other
            target = 3;

            gooberTargeter.GetComponent<Image>().color = Color.blue;
        } else
        {
            //turn on unclaimed
            target = 0;

            gooberTargeter.GetComponent<Image>().color = Color.white;
        }

        for (int i = 0; i < 4; i++)
        {
            if (i == target)
            {
                gooberStatusOverlay.transform.GetChild(i).gameObject.SetActive(true);
            } else
            {
                gooberStatusOverlay.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }



    public void HandleEffects()
    {
        if (activeEffects.Contains("adrenaline"))
        {
            StartCoroutine(AdrenalineEffect());
        }
        else if (activeEffects.Contains("dopamine"))
        {
            StartCoroutine(DopamineEffect());
        }
    }

    IEnumerator AdrenalineEffect()
    {
        m_player.walkSpeed = 12;
        m_player.sprintSpeed = 19;
        m_player.crouchSpeed = 9;
        m_player.airSpeed = 15;
        yield return new WaitForSeconds(5);
        m_player.walkSpeed = 7;
        m_player.sprintSpeed = 14;
        m_player.crouchSpeed = 4;
        m_player.airSpeed = 10;
        activeEffects.Remove("adrenaline");
        yield return null;
    }

    IEnumerator DopamineEffect()
    {
        if (health < 100)
        {
            for (int i = 0; i < 10; i++)
            {
                health += 2;
                yield return new WaitForSeconds(1);
            }
            if(health > maxHealth / 10)
            {
                vignette.DOColor(new Color32(0, 0, 0, 40), 0.75f);
            }
            activeEffects.Remove("dopamine");
            yield return null;
        }
        else
        {
            yield return null;
        }
    }

    IEnumerator DeathTimer()
    {
        inputActions.Disable();
        Debug.Log("WE");
        yield return new WaitForSeconds(5f);

        deathTimerOn = false;
        inputActions.Enable();
    }



    [PunRPC]
    public void RPC_UpdatePos(Vector3 pos)
    {
        if(!photonView.IsMine)
        {
            Debug.Log("hit button");
            transform.position = pos;
        }
    }

    [PunRPC]
    public void ApplyDamage(float damage, GameObject source)
    {
        
        health -= damage;
        if (health <= 0)
        {
            vignette.DOColor(new Color32(0, 0, 0, 40), 0.25f);
            isDead = true;
        }
        else
        {
            StartCoroutine(ShowDamageIndicator(1f, source));
            Sequence tweenSequence = DOTween.Sequence();
            tweenSequence.Append(vignette.DOColor(new Color32(255,0,0,40), 0.75f));
            if(health >= maxHealth/10) tweenSequence.Append(vignette.DOColor(new Color32(0,0,0,40), 0.25f));
            tweenSequence.Play();

        }

        /*if(isDead)
        {
            killFeedScript.player2 = username;
        }*/

        return;
    }

    public IEnumerator ShowDamageIndicator(float time, GameObject source)
    {
        damInd.SetActive(true);

        // Calculate the direction from the player's camera to the objective
        Vector3 directionToObjective = (source.transform.position - playerCamera.position).normalized;

        // Project the direction onto the horizontal plane (ignoring vertical component)
        Vector3 directionOnHorizontalPlane = Vector3.ProjectOnPlane(directionToObjective, Vector3.up).normalized;

        // Calculate the angle between the forward direction of the player's view and the direction to the objective
        float angleToObjective = Vector3.SignedAngle(playerCamera.forward, directionOnHorizontalPlane, Vector3.up);

        // Rotate the arrow UI element to point towards the objective
        damInd.transform.rotation = Quaternion.Euler(0f, 0f, -angleToObjective);

        yield return new WaitForSeconds(time);
        damInd.SetActive(false);
    }

    [PunRPC]
    public void startPointer(int i)
    {
        if (pointCollection[i].GetComponent<PointCollectorScript>().currentPlayer != null)
        {
            pointCollection[i].GetComponent<PointCollectorScript>().currentPlayer.GetComponent<PlayerManager>().buttonsPressed -= 1;
            pointCollectorBarScript.updateBar(team, -1);
        }
        pointCollection[i].GetComponent<PointCollectorScript>().runPointCollision(gameObject);
        pointsCollectedIndexList.Add(i);
    }

    [PunRPC]
    public void UpdateFlag()
    {
        goober.GetComponent<GooberFunctionality>().RunCollision(gameObject);
    }



    //ran when host clients timer hits 0, synchs all clocks
    [PunRPC]
    public void startTimer()
    {
        WaveStart timerScript = gameObject.GetComponentInChildren<WaveStart>();
    }

    //ran when host clients timer hits 0, synchs all clocks
    [PunRPC]
    public void startClock()
    {
        WaveStart timerScript = gameObject.GetComponentInChildren<WaveStart>();
        timerScript.currentTime = 0f;
        timerScript.countdownOverlay.SetActive(false);
        timerScript.Reset();
        
    }

    [PunRPC]
    public void endGame()
    {
        Debug.Log("game over");
        WaveStart waveStartScript = gameObject.GetComponentInChildren<WaveStart>();
        waveStartScript.win = true;
        waveStartScript.WinCondition(waveStartScript.winTeam); //causes returnTimer to decrease faster
        inputActions.Disable();
        inputActions.Menu.Enable();
    }

    [PunRPC]
    public void loadLevel()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Queue");
        }
        
    }

    [PunRPC]
    public void DropGoober()
    {
        foreach (GameObject a in goober.GetComponent<GooberFunctionality>().currentPlayer.GetComponent<WeaponManager>().abilityUI.abilityObjects)
        {
            a.transform.GetChild(3).gameObject.GetComponent<Image>().enabled = false;
            a.transform.GetChild(0).GetChild(1).GetComponent<Image>().enabled = true;
        }
        goober.GetComponent<GooberFunctionality>().dropped = 5.01f;
        goober.transform.SetParent(null);
        goober.transform.position += new Vector3(0, -1.5f, 0);
        goober.transform.SetParent(GameObject.Find("Goobers").transform);
        goober.GetComponent<SphereCollider>().enabled = true;
        killFeedScript.AlertFeedInstantiate(killFeedScript.boxesCount, (goober.GetComponent<GooberFunctionality>().currentPlayer.gameObject.GetComponent<PlayerManager>().username + " dropped the ATP!"));
        goober.GetComponent<GooberFunctionality>().currentPlayer = null;
        goober.GetComponent<GooberFunctionality>().team = null;
        Debug.Log("left game");
    }

    [PunRPC]
    public void synchPoints(int A, int B)
    {
        pointA.GetComponent<PointContainer>().points = A;
        pointB.GetComponent<PointContainer>().points = B;
    }

    public IEnumerator SetFullscreen(float wait, bool full)
    {
        yield return new WaitForSeconds(wait);
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, full);
    }
}