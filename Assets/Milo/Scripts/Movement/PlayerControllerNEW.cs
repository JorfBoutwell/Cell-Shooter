//Milo Reynolds

using UnityEngine;

public class PlayerControllerNEW : MonoBehaviour
{
    InputManager m_input;
    Rigidbody m_rb;
    Camera m_FPSCam;

    [Header("Movement Variables")]
    [SerializeField] float m_movementSpeed;
    Vector3 m_moveDirection;
    public bool canMove = true;
    [SerializeField] float m_groundDrag;

    [Header("Camera Variables")]
    [SerializeField] float m_sensitivityX;
    [SerializeField] float m_sensitivityY;
    [SerializeField] Transform m_orientation;
    private float m_xRot;
    private float m_yRot;

    [Header("Ground Check Variables")]
    public bool isGrounded;
    [SerializeField] float m_playerHeight;
    [SerializeField] LayerMask m_groundLayer;

    [Header("Jump Variables")]
    [SerializeField] float m_jumpForce;
    [SerializeField] float m_airMultiplier;
    [SerializeField] bool isJumping;

    [Header("Sprint/Crouch Variables")]
    public MovementState state;
    [SerializeField] float m_walkSpeed;
    [SerializeField] float m_sprintSpeed;
    [SerializeField] float m_crouchSpeed;
    public bool isSprinting = false;
    public bool isCrouching = false;
    [SerializeField] float startCamY;
    [SerializeField] float crouchCamY;

    [Header("Slope Variables")]
    [SerializeField] float m_maxSlopeAngle;
    [SerializeField] RaycastHit m_slopeHit;

    [Header("Slide Variables")]
    [SerializeField] bool m_isSliding;
    [SerializeField] float m_slideForce;
    public float m_slideTimer;
    public float maxSlideTime;


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
        air
    }

    private void Update()
    {
        HandleMovement();
        HandleCamera();
        CheckGrounded();

        StateHandler();
        HandleSliding();
    }

    private void HandleMovement()
    {
        Vector2 movementInput = m_input.inputActions.Movement.Locomotion.ReadValue<Vector2>();
        Vector3 flatVel = new Vector3(m_rb.velocity.x, 0f, m_rb.velocity.z);
        if (canMove)
        {
            m_moveDirection = m_orientation.forward * movementInput.y + m_orientation.right * movementInput.x;
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

        m_FPSCam.transform.rotation = Quaternion.Euler(m_xRot, m_yRot, 0);
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
        if (isSprinting && isCrouching && m_rb.velocity != Vector3.zero)
        {
            state = MovementState.sliding;
            m_isSliding = true;
        }
        else if (isGrounded && isSprinting && m_moveDirection != Vector3.zero)
        {
            state = MovementState.sprinting;
            m_movementSpeed = m_sprintSpeed;
            m_FPSCam.fieldOfView = Mathf.Lerp(m_FPSCam.fieldOfView, 75, Time.deltaTime * 5f);
        }
        else if (isGrounded && isCrouching)
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
            isJumping = false;
            m_FPSCam.fieldOfView = Mathf.Lerp(m_FPSCam.fieldOfView, 60, Time.deltaTime * 5f);
        }
        else
        {
            state = MovementState.air;
            isJumping = true;
        }

        if(!isCrouching)
            m_FPSCam.transform.position = Vector3.Lerp(m_FPSCam.transform.position, new Vector3(m_FPSCam.transform.position.x, transform.position.y + 0.2f, m_FPSCam.transform.position.z), Time.deltaTime * 5f);
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
}
