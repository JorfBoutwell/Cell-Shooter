//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    InputActions m_inputActions;
    CharacterController m_controller;
    Camera m_FPS_Camera;

    [Header("Movement Variables")]
    Vector3 m_playerVelocity;
    [SerializeField] float m_speed = 10f;
    [SerializeField] float m_jumpForce = 8f;
    [SerializeField] float m_gravity = 9.8f;
    private bool m_canMove = true;

    [Header("Camera Look Variables")]
    [SerializeField] float m_mouseSensitivityX = 100f;
    [SerializeField] float m_mouseSensitivityY = 75f;
    [SerializeField] float m_minViewDistance = 35f;
    [SerializeField] float m_tiltAmount = 0f;
    private float m_xRotation;

    [Header("Movement Actions Variables")]
    [SerializeField] Transform m_crouchPos;
    [SerializeField] Transform m_cameraPos;
    [SerializeField] float m_crouchYScale;

    [SerializeField] float m_slideSpeed = 25;
    [SerializeField] float m_slideTimer = 0.5f;

    [SerializeField] LayerMask m_wallRunMask;
    [SerializeField] float m_wallCheckDistance = 7f;
    [SerializeField] float m_wallRunForce = 15f;
    private bool m_wallRight = false;
    private bool m_wallLeft = false;

    private bool m_isSprinting;
    private bool m_isCrouching;
    private bool m_isSliding;
    private bool m_isWallRunning;



    private void Awake()
    {
        m_inputActions = new InputActions();
        m_controller = GetComponent<CharacterController>();
        m_FPS_Camera = Camera.main;

        m_inputActions.Movement.Jump.performed += ctx => Jump();
        m_inputActions.Movement.Sprint.performed += ctx => ToggleSprint();
        m_inputActions.Movement.Sprint.canceled += ctx => ToggleSprint();
        m_inputActions.Movement.Crouch.performed += ctx => ToggleCrouch();
        m_inputActions.Movement.Crouch.canceled += ctx => ToggleCrouch();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        m_inputActions.Enable();
    }

    private void OnDisable()
    {
        m_inputActions.Disable();
    }

    private void Update()
    {
        UpdateMovement();
        UpdateCamera();

        HandleCrouching();
        HandleSliding();
        HandleWallRun();
    }

    // Movement and Look
    private void UpdateMovement()
    {
        if (m_canMove)
        {
            Vector2 movementInput;
            movementInput = m_inputActions.Movement.Locomotion.ReadValue<Vector2>();

            if (m_isSprinting && !m_isCrouching)
                m_speed = 14f;
            else if (m_isCrouching)
                m_speed = 5f;
            else
                m_speed = 8f;

            float moveDirectionY = m_playerVelocity.y;
            m_playerVelocity = new Vector3(movementInput.x, 0, movementInput.y);
            m_playerVelocity = m_FPS_Camera.transform.TransformDirection(m_playerVelocity);
            m_playerVelocity *= m_speed;
            m_playerVelocity.y = moveDirectionY;

            if (!m_controller.isGrounded)
                m_playerVelocity.y -= m_gravity * Time.deltaTime;


            m_controller.Move(m_playerVelocity * Time.deltaTime);
        }
    }

    private void UpdateCamera()
    {
        Vector2 mouseInput;
        mouseInput = m_inputActions.Movement.Look.ReadValue<Vector2>();

        if (m_isSprinting && !m_isCrouching)
            m_FPS_Camera.fieldOfView = Mathf.Lerp(m_FPS_Camera.fieldOfView, 70, Time.deltaTime * 5);
        else if (m_isCrouching)
            m_FPS_Camera.fieldOfView = Mathf.Lerp(m_FPS_Camera.fieldOfView, 50, Time.deltaTime * 5);
        else
            m_FPS_Camera.fieldOfView = Mathf.Lerp(m_FPS_Camera.fieldOfView, 60, Time.deltaTime * 5);

        float mouseX = mouseInput.x * m_mouseSensitivityX * Time.deltaTime;
        float mouseY = mouseInput.y * m_mouseSensitivityY * Time.deltaTime;

        m_xRotation -= mouseY;
        m_xRotation = Mathf.Clamp(m_xRotation, -90f, m_minViewDistance);
        m_FPS_Camera.transform.localRotation = Quaternion.Euler(m_xRotation, 0f, m_tiltAmount);
        transform.rotation *= Quaternion.Euler(0, mouseX, 0);
    }

    // Movement Actions

    private void Jump()
    {
        if (!m_isWallRunning)
        {
            if (m_controller.isGrounded)
                m_playerVelocity.y = m_jumpForce;
        }
    }

    private void ToggleSprint()
    {
        m_isSprinting = !m_isSprinting;
    }

    private void ToggleCrouch()
    {
        m_isCrouching = !m_isCrouching;
    }

    private void HandleCrouching()
    {
        if (!m_isWallRunning)
        {
            if (m_isCrouching)
            {
                m_FPS_Camera.transform.position = Vector3.Lerp(m_FPS_Camera.transform.position, m_crouchPos.position, Time.deltaTime * 5f);
                transform.localScale = new Vector3(transform.localScale.x, m_crouchYScale, transform.localScale.z);
            }
            else
            {
                m_FPS_Camera.transform.position = Vector3.Lerp(m_FPS_Camera.transform.position, m_cameraPos.position, Time.deltaTime * 5f);
                transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
            }
        }
    }

    private void HandleSliding()
    {
        if (!m_isWallRunning)
        {
            if (m_isSprinting)
            {
                if (m_isCrouching)
                {
                    m_isSliding = true;
                    m_isCrouching = false;
                    m_isSprinting = false;
                }
            }

            if (m_isSliding)
            {
                Vector2 movementInput;
                movementInput = m_inputActions.Movement.Locomotion.ReadValue<Vector2>();

                Vector3 slideDirection = transform.forward * movementInput.y + transform.right * movementInput.x;

                transform.localScale = new Vector3(transform.localScale.x, m_crouchYScale, transform.localScale.z);
                m_controller.Move(slideDirection * m_slideSpeed * Time.deltaTime);

                m_slideTimer -= Time.deltaTime;

                if (m_slideTimer <= 0)
                {
                    transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
                    m_isSliding = false;
                    m_slideTimer = 0.5f;
                }
            }
        }
    }

    private void HandleWallRun()
    {
        RaycastHit rightWallHit;
        RaycastHit leftWallHit;

        m_wallRight = Physics.Raycast(transform.position, transform.right, out rightWallHit, m_wallCheckDistance, m_wallRunMask);
        m_wallLeft = Physics.Raycast(transform.position, -transform.right, out leftWallHit, m_wallCheckDistance, m_wallRunMask);

        if (!m_controller.isGrounded)
        {
            if (m_wallRight || m_wallLeft)
            {
                m_isWallRunning = true;
                m_speed = 8;

                Vector3 wallNormal = m_wallRight ? rightWallHit.normal : leftWallHit.normal;
                Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

                if ((transform.forward - wallForward).magnitude > (transform.forward - -wallForward).magnitude)
                    wallForward = -wallForward;

                m_controller.Move(wallForward * m_wallRunForce * Time.deltaTime);
                if (m_wallRight)
                    m_tiltAmount = Mathf.Lerp(m_tiltAmount, 5, Time.deltaTime * 5);
                else if (m_wallLeft)
                    m_tiltAmount = Mathf.Lerp(m_tiltAmount, -5, Time.deltaTime * 5);
            }
        }
        else
        {
            m_isWallRunning = false;
            m_tiltAmount = Mathf.Lerp(m_tiltAmount, 0, Time.deltaTime * 5);
        }
    }
}
