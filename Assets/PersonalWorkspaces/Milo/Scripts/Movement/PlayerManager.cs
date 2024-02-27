//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    PlayerControllerNEW m_player;
    WeaponManager m_weapon;
    Neuron m_neuron;



    [SerializeField] GameObject UI;
    [SerializeField] GameObject hitbox;

    [Header("UI References")]
    [SerializeField] GameObject damInd;

    public InputActions inputActions;

    public float health = 100;
    public int ammo;
    public int stamina; // not implemented yet
    public int style; //not implemented yet
    public string team;
    public string character;
    public bool isDead = false;

    private static readonly string TeamPropKey = "TeamA?";
    public string updatePoints;
    public int buttonsPressed;
    public List<GameObject> pointCollectors = new List<GameObject>();
    public int currentPointCollectorsA = 0;
    public List<string> activeEffects;
    public GameObject[] pointCollection;


    PhotonView view;

    [SerializeField] Material materialA;
    [SerializeField] Material materialB;

    public string username;



    private void Awake()
    {
        //username = PhotonNetwork.PlayerList[PhotonNetwork.PlayerList.Length - 1].ToString();
        username = PhotonNetwork.LocalPlayer.NickName;
        //could try targetPlayer.NickName or PhotonNetwork.LocalPlayer instead

        character = "Neuron";

        inputActions = new InputActions();
        m_player = GetComponent<PlayerControllerNEW>();
        m_weapon = GetComponent<WeaponManager>();
        m_neuron = GetComponent<Neuron>();

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

    }

    private void Start()
    {

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (photonView.Owner.ActorNumber == player.ActorNumber)
            {
                object teamA;
                player.CustomProperties.TryGetValue(TeamPropKey, out teamA);
                

                
                

                if ((bool)teamA)
                {
                    team = "A";
                    transform.GetChild(0).gameObject.layer = 11;
                    gameObject.layer = 11;
                    gameObject.GetComponent<MeshRenderer>().material = materialA;

                }
                else
                {
                    team = "B";
                    transform.GetChild(0).gameObject.layer = 13;
                    gameObject.layer = 13;
                    gameObject.GetComponent<MeshRenderer>().material = materialB;
                }

            }
        }
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

        if (isDead)
        {
            inputActions.Disable();
        }
        else
        {
            inputActions.Enable(); //this seems slow. better way?
        }
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
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "PointCollector")
    //    {
    //        if (buttonsPressed >= 0)
    //        {
    //            pointCollectors.Add(collision.gameObject as GameObject);
    //            Debug.Log("Yeah" + pointCollectors[0]);
    //            view.RPC("RPC_UpdatePos", RpcTarget.AllBuffered, gameObject.transform.position);
    //        }
    //    }

    //}

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
        m_player.walkSpeed = 14;
        m_player.sprintSpeed = 28;
        m_player.crouchSpeed = 8;
        m_player.airSpeed = 20;
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
        while (health < 120)
        {
            health = health + 2;
            yield return new WaitForSeconds(1);
        }
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
            isDead = true;
        }
        else
        {
            StartCoroutine(ShowDamageIndicator(1f, source));
        }

        return;
    }

    public IEnumerator ShowDamageIndicator(float time, GameObject source)
    {
        damInd.SetActive(true);
        Vector3 dirToSource = source.transform.position - transform.position;
        dirToSource.y = 0;
        damInd.transform.rotation = Quaternion.LookRotation(dirToSource);
        yield return new WaitForSeconds(time);
        damInd.SetActive(false);
    }

    [PunRPC]
    public void startPointer(int i)
    {
        if (pointCollection[i].GetComponent<PointCollectorScript>().currentPlayer != null)
        {
            pointCollection[i].GetComponent<PointCollectorScript>().currentPlayer.GetComponent<PlayerManager>().buttonsPressed -= 1;
        }
            pointCollection[i].GetComponent<PointCollectorScript>().runPointCollision(gameObject);
        
        
        
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
        timerScript.currentTime = 0;
        timerScript.countdownOverlay.SetActive(false);
        timerScript.Reset();

    }

    [PunRPC]
    public void endGame()
    {

        WaveStart waveStartScript = gameObject.GetComponentInChildren<WaveStart>();
        waveStartScript.WinCondition(waveStartScript.winTeam); //causes returnTimer to decrease faster
        waveStartScript.win = true;
    }
}