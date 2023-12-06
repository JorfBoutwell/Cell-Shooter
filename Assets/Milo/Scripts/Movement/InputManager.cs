//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
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

        inputActions.Weapon.Fire.performed += ctx => m_weapon.isShooting = true;
        //inputActions.Weapon.Fire.canceled += ctx => m_weapon.isShooting = false;
        inputActions.Weapon.Reload.performed += ctx => m_weapon.StartCoroutine("Reload");
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
