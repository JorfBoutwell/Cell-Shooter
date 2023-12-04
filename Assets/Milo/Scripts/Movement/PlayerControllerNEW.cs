//Milo Reynolds

using UnityEngine;

public class PlayerControllerNEW : MonoBehaviour
{
    InputManager m_input;
    Rigidbody m_rb;
    Camera m_FPSCam;

    public MovementState state;

    [Header("Movement Speed Variables")]
    [SerializeField] float m_movementSpeed;
    [SerializeField] float m_walkSpeed;
    [SerializeField] float m_sprintSpeed;
    [SerializeField] float m_crouchSpeed;
    [SerializeField] float m_wallRunSpeed;

    [Header("Movement Flags")]
    public bool canMove = true;
    public bool isGrounded;
    [SerializeField] bool m_isJumping;
    [SerializeField] bool m_isSprinting;
    [SerializeField] bool m_isCrouching;
    [SerializeField] bool m_isSliding;
    [SerializeField] bool m_isWallRunning;

    [Header("Movement Variables")]
    Vector3 m_moveDirection;
    Vector3 m_moveInput;
    [SerializeField] float m_groundDrag;

    [Header("Camera Variables")]
    [SerializeField] float m_sensitivityX;
    [SerializeField] float m_sensitivityY;
    [SerializeField] Transform m_orientation;
    [SerializeField] float m_tilt;
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
    [SerializeField] float m_maxWallRunTime;
    [SerializeField] float m_wallRunTimer;
    [SerializeField] float m_wallCheckDistance;
    [SerializeField] float m_minJumpHeight;
    private RaycastHit m_leftWallHit;
    private RaycastHit m_rightWallHit;
    private bool m_wallLeft;
    private bool m_wallRight;


    void Awake()
    {
        m_input = GetComponent<InputManager>();
        m_rb = GetComponent<Rigidbody>();
        m_FPSCam = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        startCamY = transform.position.y;
        crouchCamY = transform.position.y - 0.3f;

        m_rb.freezeRotation = true;

        m_slideTimer = maxSlideTime;
    }

    public enum MovementState
    {
        idle,
        walking,
        sprinting,
        crouching,
        sliding,
        wallrunning,
        air
    }

    private void Update()
    {
        HandleMovement();
        HandleCamera();
        CheckGrounded();
        CheckForWall();

        StateHandler();
        HandleSliding();
        HandleWallRunning();
    }

    private void HandleMovement()
    {
        m_moveInput = m_input.inputActions.Movement.Locomotion.ReadValue<Vector2>();
        Vector3 flatVel = new Vector3(m_rb.velocity.x, 0f, m_rb.velocity.z);
        if (canMove)
        {
            m_moveDirection = m_orientation.forward * m_moveInput.y + m_orientation.right * m_moveInput.x;
            m_groundDrag = 5;
        }

        if (OnSlope() && !m_isJumping)
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
            m_rb.AddForce(m_moveDirection.normalized * m_movementSpeed * 10f, ForceMode.Force);
        }
        else
        {
            m_rb.drag = 0;
            m_rb.AddForce(m_moveDirection.normalized * m_movementSpeed * 10f * m_airMultiplier, ForceMode.Force);
        }

        m_rb.useGravity = !OnSlope();

        if (flatVel.magnitude > m_movementSpeed && !OnSlope())
        {
            Vector3 limitedVel = flatVel.normalized * m_movementSpeed;
            m_rb.velocity = new Vector3(limitedVel.x, m_rb.velocity.y, limitedVel.z);
        }
    }

    private void HandleCamera()
    {
        Vector2 mouseInput;
        mouseInput = m_input.inputActions.Movement.Look.ReadValue<Vector2>();

        m_yRot += (mouseInput.x * (m_sensitivityX/100));
        m_xRot -= (mouseInput.y * (m_sensitivityY/100));
        m_xRot = Mathf.Clamp(m_xRot, -90f, 90f);

        m_FPSCam.transform.rotation = Quaternion.Euler(m_xRot, m_yRot, m_tilt);
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

    private void StateHandler()
    {
        if ((m_wallLeft || m_wallRight) && m_moveInput.y > 0 && !isGrounded)
        {
            state = MovementState.wallrunning;
            m_isWallRunning = true;
        }
        else if (m_isSprinting && m_isCrouching && m_rb.velocity != Vector3.zero)
        {
            state = MovementState.sliding;
            m_isSliding = true;
        }
        else if (isGrounded && m_isSprinting && m_moveDirection != Vector3.zero)
        {
            state = MovementState.sprinting;
            m_movementSpeed = m_sprintSpeed;
            m_FPSCam.fieldOfView = Mathf.Lerp(m_FPSCam.fieldOfView, 75, Time.deltaTime * 5f);
        }
        else if (isGrounded && m_isCrouching)
        {
            state = MovementState.crouching;
            m_movementSpeed = m_crouchSpeed;
            m_FPSCam.transform.position = Vector3.Lerp(m_FPSCam.transform.position, new Vector3(m_FPSCam.transform.position.x, crouchCamY, m_FPSCam.transform.position.z), Time.deltaTime * 5f);
            m_FPSCam.fieldOfView = Mathf.Lerp(m_FPSCam.fieldOfView, 55, Time.deltaTime * 5f);
        }
        else if(isGrounded)
        {
            state = MovementState.walking;
            m_movementSpeed = m_walkSpeed;
            m_isJumping = false;
            m_FPSCam.fieldOfView = Mathf.Lerp(m_FPSCam.fieldOfView, 60, Time.deltaTime * 5f);
        }
        else
        {
            state = MovementState.air;
            m_isJumping = true;
        }

        if(!m_isCrouching)
            m_FPSCam.transform.position = Vector3.Lerp(m_FPSCam.transform.position, new Vector3(m_FPSCam.transform.position.x, transform.position.y + 0.2f, m_FPSCam.transform.position.z), Time.deltaTime * 5f);
    }

    public void ToggleSprint()
    {
        m_isSprinting = !m_isSprinting;
    }

    public void ToggleCrouch()
    {
        m_isCrouching = !m_isCrouching;
    }

    public void Jump()
    {
        if(isGrounded)
        {
            m_rb.velocity = new Vector3(m_rb.velocity.x, 0f, m_rb.velocity.z);

            m_rb.AddForce(transform.up * m_jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleSliding()
    {
        if(m_isSliding && m_slideTimer > 0)
        {
            m_FPSCam.transform.position = Vector3.Lerp(m_FPSCam.transform.position, new Vector3(m_FPSCam.transform.position.x, crouchCamY, m_FPSCam.transform.position.z), Time.deltaTime * 10f);
            m_rb.AddForce(m_orientation.forward.normalized * m_slideForce, ForceMode.Force);
            canMove = false;
            m_slideTimer -= Time.deltaTime;
        }
        else
        {
            m_slideTimer = maxSlideTime;
            canMove = true;
            m_isSliding = false;
        }
    }

    private void CheckForWall()
    {
        m_wallRight = Physics.Raycast(transform.position, m_orientation.right, out m_rightWallHit, m_wallCheckDistance, m_wallLayer);
        m_wallLeft = Physics.Raycast(transform.position, -m_orientation.right, out m_leftWallHit, m_wallCheckDistance, m_wallLayer);
    }

    private void HandleWallRunning()
    {
        if(state == MovementState.wallrunning)
        {
            if(m_wallRunTimer > 0)
            {
                m_rb.useGravity = false;
                m_rb.velocity = new Vector3(m_rb.velocity.x, 0f, m_rb.velocity.z);
                m_movementSpeed = m_wallRunSpeed;
                m_wallRunTimer -= Time.deltaTime;

                Vector3 wallNormal = m_wallRight ? m_rightWallHit.normal : m_leftWallHit.normal;
                Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

                if ((m_orientation.forward - wallForward).magnitude > (m_orientation.forward - -wallForward).magnitude)
                    wallForward = -wallForward;

                m_rb.AddForce(wallForward * m_wallRunForce, ForceMode.Force);

                if (!(m_wallLeft && m_moveInput.x > 0) && !(m_wallRight && m_moveInput.x < 0))
                    m_rb.AddForce(-wallNormal * 100, ForceMode.Force);

                if (m_wallRight)
                    m_tilt = Mathf.Lerp(m_tilt, 5, Time.deltaTime * 5);
                else if (m_wallLeft)
                    m_tilt = Mathf.Lerp(m_tilt, -5, Time.deltaTime * 5);
            }
        }
        else
        {
            m_wallRunTimer = m_maxWallRunTime;
            m_isWallRunning = false;
            m_rb.useGravity = true;
            m_tilt = Mathf.Lerp(m_tilt, 0, Time.deltaTime * 5);
        }
    }
}
