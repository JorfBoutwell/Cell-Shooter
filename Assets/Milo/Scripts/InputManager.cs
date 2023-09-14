//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    InputActions inputActions;

    public Vector2 movementInput;

    private void Awake()
    {
        inputActions = new InputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        movementInput = inputActions.Movement.Locomotion.ReadValue<Vector2>();
    }
}
