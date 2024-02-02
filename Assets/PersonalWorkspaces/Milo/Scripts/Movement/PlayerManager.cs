//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPun
{
    PlayerControllerNEW m_player;
    WeaponManager m_weapon;
    AbilityManager m_abilityManger;
    Neuron m_neuron;

    

    [SerializeField] GameObject UI;
    [SerializeField] GameObject hitbox;

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
    PhotonView view;

    private void Awake()
    {
        

        character = "Neuron";

        inputActions = new InputActions();
        m_player = GetComponent<PlayerControllerNEW>();
        m_weapon = GetComponent<WeaponManager>();
        m_abilityManger = GetComponent<AbilityManager>();
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
            foreach(Canvas canvas in canvases)
            {
                Destroy(canvas);
            }


        }
    }

    private void Start()
    {
        //set team layer
        object teamA;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(TeamPropKey, out teamA) && view.IsMine)
        {
            if ((bool)teamA)
            {
                view.RPC("RPC_TakeDamage", RpcTarget.OthersBuffered, 11);
                team = "A";
                transform.GetChild(0).gameObject.layer = 11;
                gameObject.layer = 11;

            }
            else
            {
                Debug.Log("team B");
                transform.GetChild(0).gameObject.layer = 13;
                gameObject.layer = 13;
                view.RPC("RPC_TakeDamage", RpcTarget.OthersBuffered, 13);
                team = "B";
            }
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

        if(isDead)
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

    [PunRPC]
    public void ApplyDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            isDead = true;
        }

        return;
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

        inputActions.Ability.Ability1.performed += ctx => m_abilityManger.currentAbility = m_abilityManger.abilityList[0];
        inputActions.Ability.Ability2.performed += ctx => m_abilityManger.currentAbility = m_abilityManger.abilityList[1];
        inputActions.Ability.Ability3.performed += ctx => m_abilityManger.currentAbility = m_abilityManger.abilityList[2];
        inputActions.Ability.Ability3.performed += ctx => m_abilityManger.currentAbility = m_abilityManger.abilityList[3];
        inputActions.Weapon.Melee.performed += ctx => m_abilityManger.currentAbility = m_abilityManger.abilityList[4];
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    [PunRPC]
    void RPC_ChangeLayer(int Layer)
    {

        if (photonView.IsMine)
        {
            gameObject.layer = Layer;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "PointCollector")
        {
          if (buttonsPressed > 0) { 
              pointCollectors.Add(collision.gameObject as GameObject);
              Debug.Log("Yeah" + pointCollectors[0]);
          }
        }
    }
}
