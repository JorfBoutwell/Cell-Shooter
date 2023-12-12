//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Netcode;

public class PlayerControllerNEW : NetworkBehaviour
{
    InputManager m_input;
    Rigidbody m_rb;
    Camera m_FPSCam;

    public MovementState state;

    public AnimationController animController;

    [Header("Movement Speed Variables")]
    [SerializeField] float m_movementSpeed;
    [SerializeField] float m_walkSpeed;
    [SerializeField] float m_sprintSpeed;
    [SerializeField] float m_crouchSpeed;

    [Header("Movement Flags")]
    public bool canMove = true;
    public bool isGrounded;
    public bool isJumping;
    public bool isSprinting;
    public bool isCrouching;
    public bool isSliding;
    public bool isWallRunning;

    [Header("Movement Variables")]
    Vector3 m_moveDirection;
    Vector3 m_moveInput;
    [SerializeField] float m_groundDrag;

    [Header("Camera Variables")]
    [SerializeField] float m_sensitivityX;
    [SerializeField] float m_sensitivityY;
    [SerializeField] Transform m_camHolder;
    [SerializeField] Transform m_orientation;
    private float m_xRot;
    private float m_yRot;

    [Header("Ground Check Variables")]
    [SerializeField] float m_playerHeight;
    [SerializeField] LayerMask m_groundLayer;

    [Header("Jump Variables")]
    [SerializeField] float m_jumpForce;
    [SerializeField] float m_airMultiplier;
    
    [Header("Sprint/Crouch Variables")]
    [SerializeField] float startCamY;
    [SerializeField] float crouchCamY;

    [Header("Slope Variables")]
    [SerializeField] float m_maxSlopeAngle;
    [SerializeField] RaycastHit m_slopeHit;

    [Header("Slide Variables")]
    [SerializeField] float m_slideForce;
    public float m_slideTimer;
    public float maxSlideTime;

    [Header("Wallrun Variables")]
    [SerializeField] LayerMask m_wallLayer;
    [SerializeField] float m_wallRunForce;
    [SerializeField] float m_wallJumpUpForce;
    [SerializeField] float m_wallJumpSideForce;
    [SerializeField] float m_maxWallRunTime;
    [SerializeField] float m_exitWallTime;
    [SerializeField] float m_wallCheckDistance;
    [SerializeField] bool m_useGravity;
    [SerializeField] float m_gravityCounterForce;
    public float m_wallRunTimer;
    private RaycastHit m_leftWallHit;
    private RaycastHit m_rightWallHit;
    public bool m_wallLeft;
    public bool m_wallRight;
    public bool m_exitingWall;
    public float m_exitWallTimer;
    
    


    void Awake()
    {
        m_input = GetComponent<InputManager>();
        m_rb = GetComponent<Rigidbody>();
        animController = GetComponentInChildren<AnimationController>();
        m_FPSCam = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        startCamY = transform.position.y;
        crouchCamY = transform.position.y - 0.3f;

        m_rb.freezeRotation = true;

        m_slideTimer = maxSlideTime;

        m_wallRunTimer = m_maxWallRunTime;
        m_exitWallTimer = m_exitWallTime;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Destroy(this.gameObject.transform.GetChild(1).transform.GetChild(0).gameObject);
            Destroy(this);
        }
    }

    public enum MovementState
    {
        idle,
        walking,
        sprinting,
        crouching,
        sliding,
        wallrunning,
        exitingwall,
        air
    }

    private void Update()
    {
        HandleMovement();
        HandleCamera();
        CheckGrounded();
        CheckForWall();

        StateMachine();
        HandleSliding();

        if(isWallRunning)
            HandleWallRunning();
    }

    private void HandleMovement()
    {
        m_moveInput = m_input.inputActions.Movement.Locomotion.ReadValue<Vector2>() * Time.deltaTime;
        Vector3 flatVel = new Vector3(m_rb.velocity.x, 0f, m_rb.velocity.z);
        if (canMove)
        {
            m_moveDirection = m_orientation.forward * m_moveInput.y + m_orientation.right * m_moveInput.x;
            m_groundDrag = 5;
        }

        if (OnSlope() && !isJumping)
        {
            m_rb.AddForce(GetSlopeMoveDirection() * 20f, ForceMode.Force);

            if (m_rb.velocity.y > 0)
                m_rb.AddForce(Vector3.down * 40f, ForceMode.Force);

            if (m_rb.velocity.magnitude > m_movementSpeed)
            {
                m_rb.velocity = m_rb.velocity.normalized * m_movementSpeed;
            }
        }

        if (isGrounded)
        {
            m_rb.drag = m_groundDrag;
            m_rb.AddForce(m_moveDirection.normalized * m_movementSpeed, ForceMode.Force);
        }
        else
        {
            m_rb.drag = 0;
            m_rb.AddForce(m_moveDirection.normalized * m_movementSpeed * m_airMultiplier, ForceMode.Force);
        }

        if(!isWallRunning)
            m_rb.useGravity = !OnSlope();

        if (flatVel.magnitude > m_movementSpeed && !OnSlope())
        {
            Vector3 limitedVel = flatVel.normalized * m_movementSpeed;
            m_rb.velocity = new Vector3(limitedVel.x, m_rb.velocity.y, limitedVel.z);
        }
    }

    private void HandleCamera()
    {
        Vector2 mouseInput = m_input.inputActions.Movement.Look.ReadValue<Vector2>();

        m_yRot += (mouseInput.x * (m_sensitivityX/100));
        m_xRot -= (mouseInput.y * (m_sensitivityY/100));
        m_xRot = Mathf.Clamp(m_xRot, -90f, 90f);

        m_camHolder.transform.rotation = Quaternion.Euler(m_xRot, m_yRot, 0);
        m_orientation.rotation = Quaternion.Euler(0, m_yRot, 0);
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, m_playerHeight * 0.5f + 0.2f, m_groundLayer);
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out m_slopeHit, m_playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, m_slopeHit.normal);
            return angle < m_maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(m_moveDirection, m_slopeHit.normal).normalized;
    }

    /*Movement Actions*/

    private void StateMachine()
    {
        MovementState tempState = state;

        if ((m_wallLeft || m_wallRight) && m_moveInput.y > 0 && !isGrounded && !m_exitingWall)
        {
            state = MovementState.wallrunning;
            if (!isWallRunning)
                StartWallRun();

            if (m_wallRunTimer > 0)
                m_wallRunTimer -= Time.deltaTime;
                

            if (m_wallRunTimer <= 0 && isWallRunning)
            {
                m_exitingWall = true;
                m_exitWallTimer = m_exitWallTime;
            }
        }
        else if(m_exitingWall)
        {
            state = MovementState.exitingwall;
            if (isWallRunning)
                StopWallRun();

            if (m_exitWallTimer > 0)
            {
                m_exitWallTimer -= Time.deltaTime;
            }

            if (m_exitWallTimer <= 0)
            {
                m_wallRunTimer = m_maxWallRunTime;
                m_exitingWall = false;
            }

        }
        else if (isSprinting && isCrouching && m_rb.velocity != Vector3.zero)
        {
            state = MovementState.sliding;
            isSliding = true;
        }
        else if (isGrounded && isSprinting && m_moveDirection != Vector3.zero)
        {
            state = MovementState.sprinting;
            m_movementSpeed = m_sprintSpeed;
           // DoFOV(90f);
        }
        else if (isGrounded && isCrouching)
        {
            state = MovementState.crouching;
            m_movementSpeed = m_crouchSpeed;
           // DoCrouch(-0.5f);
           // DoFOV(70f);
        }
        else if(isGrounded)
        {
            state = MovementState.walking;
            m_movementSpeed = m_walkSpeed;
            isJumping = false;
           // DoFOV(80f);
        }
        else
        {
            state = MovementState.air;
            isWallRunning = false;
            isJumping = true;
           // DoFOV(80f);
           // DoTilt(0f);
        }

        if (!isCrouching)
            DoCrouch(0f);

        //if state has changed, play new animation
        if(tempState != state) 
        {
            animController.MovementAnimationController(state);
        }
    }
    //Do either togglesprint or crouch do anything?
    public void ToggleSprint()
    {
        isSprinting = !isSprinting;
    }

    public void ToggleCrouch()
    {
        isCrouching = !isCrouching;
    }

    public void Jump()
    {
        if(isGrounded)
        {
            m_rb.velocity = new Vector3(m_rb.velocity.x, 0f, m_rb.velocity.z);

            m_rb.AddForce(transform.up * m_jumpForce, ForceMode.Impulse);
        }
        if(isWallRunning)
        {
            WallJump();
        }
    }

    private void HandleSliding()
    {
        if(isSliding && m_slideTimer > 0)
        {
            DoCrouch(-0.8f);
            m_rb.AddForce(m_orientation.forward.normalized * m_slideForce, ForceMode.Force);
            canMove = false;
            m_slideTimer -= Time.deltaTime;
        }
        else
        {
            m_slideTimer = maxSlideTime;
            canMove = true;
            isSliding = false;
        }
    }

    private void CheckForWall()
    {
        m_wallRight = Physics.Raycast(transform.position, m_orientation.right, out m_rightWallHit, m_wallCheckDistance, m_wallLayer);
        m_wallLeft = Physics.Raycast(transform.position, -m_orientation.right, out m_leftWallHit, m_wallCheckDistance, m_wallLayer);
    }


    private void StartWallRun()
    {
        isWallRunning = true;
        m_rb.velocity = new Vector3(m_rb.velocity.x, 0f, m_rb.velocity.z);

       // DoFOV(90f);
        //if(m_wallLeft)
           // DoTilt(-5f);
       // if (m_wallRight)
           // DoTilt(5f);
    }

    private void HandleWallRunning()
    {
        m_rb.useGravity = m_useGravity;

        Vector3 wallNormal = m_wallRight ? m_rightWallHit.normal : m_leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((m_orientation.forward - wallForward).magnitude > (m_orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        m_rb.AddForce(wallForward * m_wallRunForce, ForceMode.Force);

        if (!(m_wallLeft && m_moveInput.x > 0) && !(m_wallRight && m_moveInput.x < 0))
            m_rb.AddForce(-wallNormal * 100, ForceMode.Force);

        if (m_rb.useGravity)
            m_rb.AddForce(transform.up * -m_gravityCounterForce, ForceMode.Force);

        if (isWallRunning && isGrounded)
            m_exitingWall = true;
    }

    private void StopWallRun()
    {
        isWallRunning = false;

       // DoFOV(80f);
       // DoTilt(0f);
    }

    private void WallJump()
    {
        m_exitingWall = true;
        m_exitWallTimer = m_exitWallTime;

        Vector3 wallNormal = m_wallRight ? m_rightWallHit.normal : m_leftWallHit.normal;

        Vector3 forceToApply = transform.up * m_wallJumpUpForce + wallNormal * m_wallJumpSideForce;

        m_rb.velocity = new Vector3(m_rb.velocity.x, 0f, m_rb.velocity.z);
        m_rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    public void DoFOV(float endValue)
    {
        m_FPSCam.GetComponent<Camera>().DOFieldOfView(endValue, 0.5f);
    }

    public void DoTilt(float zTilt)
    {
        m_FPSCam.transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.5f);
    }

    public void DoCrouch(float moveAmount)
    {
        m_FPSCam.transform.DOLocalMoveY(moveAmount, 0.5f);
        if(moveAmount == 0)
            transform.DOScaleY(1, 0.5f);
    }
}
