//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class InputManager : NetworkBehaviour
{
    public InputActions inputActions;
    PlayerControllerNEW m_player;
    WeaponManager m_weapon;
    Neuron m_neuron;

    private void Awake()
    {
        inputActions = new InputActions();
        m_player = GetComponent<PlayerControllerNEW>();
        m_weapon = GetComponent<WeaponManager>();
        m_neuron = GetComponent<Neuron>();

        
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

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) Destroy(this);
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
