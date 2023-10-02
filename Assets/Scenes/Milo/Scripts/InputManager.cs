//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputActions inputActions;
    PlayerController m_player;
    WeaponManager m_weapon;

    private void Awake()
    {
        inputActions = new InputActions();
        m_player = GetComponent<PlayerController>();
        m_weapon = GetComponent<WeaponManager>();

        inputActions.Movement.Jump.performed += ctx => m_player.Jump();
        inputActions.Movement.Sprint.performed += ctx => m_player.ToggleSprint();
        inputActions.Movement.Sprint.canceled += ctx => m_player.ToggleSprint();
        inputActions.Movement.Crouch.performed += ctx => m_player.ToggleCrouch();
        inputActions.Movement.Crouch.canceled += ctx => m_player.ToggleCrouch();

        inputActions.Weapon.Fire.performed += ctx => m_weapon.isShooting = true;
        inputActions.Weapon.Fire.canceled += ctx => m_weapon.isShooting = false;
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