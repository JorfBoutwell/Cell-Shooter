//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Photon.Pun;

public class PlayerManager : NetworkBehaviour
{
    PlayerControllerNEW m_player;
    WeaponManager m_weapon;
    Neuron m_neuron;

    [SerializeField] GameObject UI;

    public InputActions inputActions;

    public float health = 100;
    public int ammo;
    public int stamina; // not implemented yet
    public int style; //not implemented yet
    public string team;
    public string character;
    public bool isDead = false;

    PhotonView view;

    private void Awake()
    {
        team = "A";
        character = "Neuron";

        inputActions = new InputActions();
        m_player = GetComponent<PlayerControllerNEW>();
        m_weapon = GetComponent<WeaponManager>();
        m_neuron = GetComponent<Neuron>();

        ammo = m_weapon.currentWeapon.maxAmmo;

        AssignInputs();

        view = GetComponent<PhotonView>();
        if (!view.IsMine) Destroy(this);
    }

    private void Update()
    {
        if(ammo != m_weapon.currentAmmo)
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
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}