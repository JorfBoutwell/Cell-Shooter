//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class PlayerControllerNEW : MonoBehaviourPun
{
    PlayerManager m_input;
    public Rigidbody m_rb;
    [SerializeField] Camera m_FPSCam;

    public MovementState state;

    public AnimationController animController;

    [Header("Movement Speed Variables")]
    public float movementSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float airSpeed;
    public float speedDampDuration;

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
    public Vector3 moveInput;
    [SerializeField] float m_groundDrag;

    [Header("Camera Variables")]
    [SerializeField] float m_sensitivityX;
    [SerializeField] float m_sensitivityY;
    [SerializeField] Transform m_camHolder;
    private Transform m_orientation;
    private float m_xRot;
    private float m_yRot;

    [Header("Ground Check Variables")]
    [SerializeField] float m_playerHeight;
    [SerializeField] LayerMask m_groundLayer;

    [Header("Slope Variables")]
    [SerializeField] float m_maxSlopeAngle;
    private RaycastHit m_slopeHit;
    [SerializeField] Vector3 m_slopeMoveDirection;
    [SerializeField] float m_slopeDownForce;

    [Header("Jump Variables")]
    [SerializeField] float m_jumpForce;
    
    [Header("Sprint/Crouch Variables")]
    [SerializeField] float startCamY;
    [SerializeField] float crouchCamY;

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
    private bool m_wallLeft;
    private bool m_wallRight;
    public bool m_exitingWall;
    public float m_exitWallTimer;

    PhotonView view;
    
    


    void Awake()
    {
        m_input = GetComponent<PlayerManager>();
        m_rb = GetComponent<Rigidbody>();
        animController = GetComponentInChildren<AnimationController>();
        m_FPSCam = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        startCamY = transform.position.y;
        crouchCamY = transform.position.y - 0.3f;

        m_rb.freezeRotation = true;

        m_slideTimer = maxSlideTime;

        m_wallRunTimer = m_maxWallRunTime;
        m_exitWallTimer = m_exitWallTime;

        m_orientation = gameObject.transform.GetChild(1).GetChild(2);

        view = GetComponent<PhotonView>();
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
        //stops people from controlling eachother
        if (!view.IsMine)
        {
            return;
        }
        
        HandleMovement();
        HandleCamera();
        CheckGrounded();
        CheckForWall();

        StateMachine();
        HandleSliding();

        m_slopeMoveDirection = Vector3.ProjectOnPlane(m_moveDirection, m_slopeHit.normal);

        if (isWallRunning)
            HandleWallRunning();

        //tie FOV to movement speed
        //        if (m_FPSCam.fieldOfView != (m_movementSpeed * (10 / 7) + 70))
        //     m_FPSCam.DOFieldOfView(m_movementSpeed * (10 / 7) + 70, 0.2f);
    }

    private void HandleMovement()
    {
        moveInput = m_input.inputActions.Movement.Locomotion.ReadValue<Vector2>();

        

        Vector3 flatVel = new Vector3(m_rb.velocity.x, 0f, m_rb.velocity.z);
        if (canMove)
        {
            m_moveDirection = m_orientation.forward * moveInput.y + m_orientation.right * moveInput.x;
            m_groundDrag = 5;
        }


        if (isGrounded && !OnSlope())
        {
            m_rb.drag = m_groundDrag;
            m_rb.AddForce(m_moveDirection.normalized * movementSpeed, ForceMode.Acceleration);
            m_rb.useGravity = true;
        }
        else if (isGrounded && OnSlope())
        {
            m_rb.drag = m_groundDrag;
            m_rb.AddForce(m_slopeMoveDirection.normalized * movementSpeed, ForceMode.Acceleration);
            m_rb.useGravity = false;
            if(m_moveDirection != Vector3.zero)
                m_rb.AddForce(Vector3.down * m_slopeDownForce, ForceMode.Force);
                
        }
        else if (!isGrounded)
        {
            m_rb.drag = 0;
            m_rb.AddForce(m_moveDirection.normalized * movementSpeed, ForceMode.Acceleration);
            m_rb.useGravity = true;
        }

        if (flatVel.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
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

    /*Movement Actions*/

    private void StateMachine()
    {
        MovementState tempState = state;

        if ((m_wallLeft || m_wallRight) && moveInput.y > 0 && !isGrounded && !m_exitingWall)
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
            airSpeed = sprintSpeed - 2;
            DOTween.To(() => movementSpeed, x => movementSpeed = x, sprintSpeed, speedDampDuration);
           // DoFOV(90f);
        }
        else if (isGrounded && isCrouching)
        {
            state = MovementState.crouching;
            movementSpeed = crouchSpeed;
           // DoCrouch(-0.5f);
           // DoFOV(70f);
        }
        else if(isGrounded)
        {
            if(m_rb.velocity != Vector3.zero)
            {
                state = MovementState.walking;
                airSpeed = walkSpeed - 2;
                DOTween.To(() => movementSpeed, x => movementSpeed = x, walkSpeed, speedDampDuration);
            }
            else
            {
                state = MovementState.idle;
            }

            isJumping = false;
           // DoFOV(80f);
        }
        else
        {
            state = MovementState.air;
            movementSpeed = airSpeed;
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
            animController.MovementAnimationController(state, m_wallLeft, m_wallRight);
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

        if (!(m_wallLeft && moveInput.x > 0) && !(m_wallRight && moveInput.x < 0))
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
      //  m_FPSCam.GetComponent<Camera>().DOFieldOfView(endValue, 0.5f);
    }

    public void DoTilt(float zTilt)
    {
    //    m_FPSCam.transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.5f);
    }

    public void DoCrouch(float moveAmount)
    {
  //      m_FPSCam.transform.DOLocalMoveY(moveAmount, 0.5f);
//        if(moveAmount == 0)
         //   transform.DOScaleY(1, 0.5f);
    }
}
