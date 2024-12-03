using System.Collections;
using UnityEngine;

public class ThirdPers_PlayerController : MonoBehaviour
{
    [Header("Player Basic Parameters")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float rollingSpeed = 10f;
    public float rotationSpeed = 180f;
    public float gravityScale = 2f;
    public bool isAttacking = false;
    private CharacterController controller;
    private Transform cameraTransform;
    private Vector3 movement;
    private Vector3 velocity;
    private float currentSpeed;

    // Current rotation of the player
    private float currentRotationY;

    [SerializeField] private bool isGrounded;

    // Jump parameters
    [SerializeField] private float baseGravity = -9.81f;
    private float currentGravity;
    [SerializeField] private float jumpForce = 6f;
    public bool isJumping;
    public bool isRolling;

    [Header("Animation")]
    public Animator playerAnim;

// ----------------------------------------------------------------------------------------- //

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        currentRotationY = transform.eulerAngles.y;
    }

    void Update()
    {
        if (!isAttacking)
        {
            Movement();
            Rolling();
            Jump();
        }
        Attacking();
        Gravity();

        // Apply movement and gravity
        controller.Move((movement * currentSpeed + velocity) * Time.deltaTime);
    }

// ----------------------------------------------------------------------------------------- //

    // Public method to add rotation from camera
    public void AddRotation(float rotationAmount)
    {
        currentRotationY += rotationAmount;
        ApplyRotation();
    }

    void Movement()
    {
        // Get vertical input for forward/backward movement
        float vertical = Input.GetAxisRaw("Vertical");
        
        // Get rotation input (A/D keys)
        float rotationInput = Input.GetAxisRaw("Horizontal");
        
        // Get strafe input (Q/E keys)
        float strafeLeft = Input.GetKey(KeyCode.Q) ? -1 : 0;
        float strafeRight = Input.GetKey(KeyCode.E) ? 1 : 0;
        float strafe = strafeLeft + strafeRight;

        // Update rotation from keyboard input
        currentRotationY += rotationInput * rotationSpeed * Time.deltaTime;
        ApplyRotation();

        // Calculate movement direction
        Vector3 forward = transform.forward * vertical;
        Vector3 right = transform.right * strafe;
        movement = (forward + right).normalized;

        // Sprint feature
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isSprinting ? sprintSpeed : moveSpeed;
        currentSpeed = isRolling ? rollingSpeed : currentSpeed;

        // Animation handling
        if ((movement.magnitude >= 0.1f || Mathf.Abs(rotationInput) > 0.1f) && isGrounded)
        {
            playerAnim.SetBool("running", true);
        }
        else
        {
            playerAnim.SetBool("running", false);
        }
    }

    private void ApplyRotation()
    {
        // Apply the rotation smoothly
        Quaternion targetRotation = Quaternion.Euler(0, currentRotationY, 0);
        transform.rotation = targetRotation;
    }

    void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;

            if (isJumping)
            {
                isGrounded = false;
                velocity.y = 0;
                velocity.y += jumpForce;

                controller.Move(velocity * Time.deltaTime);
            }

            playerAnim.SetBool("jumping", true);
        }
        else if (!isGrounded)
        {
            playerAnim.SetBool("jumping", false);
            playerAnim.SetBool("falling", true);
        }
    }

    void Gravity()
    {
        if (!isGrounded)
        {
            if (isJumping)
            {
                isJumping = false;
            }

            // Apply higher gravity when falling
            if (velocity.y < 0)
            {
                currentGravity = baseGravity * gravityScale;
            }
            else
            {
                currentGravity = baseGravity; // Normal gravity when jumping upwards
            }
        }
        else
        {
            currentGravity = baseGravity; // Normal gravity when grounded
        }

        velocity.y += currentGravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime); // Apply gravity
    }
    
    void Attacking()
    {
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            isAttacking = true;
            playerAnim.SetBool("attack2H", true);

            // Start the coroutine to handle the attack duration
            StartCoroutine(WaitForAnimation());
        }
    }

    void Rolling()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.R))
        {
            isRolling = true;
            playerAnim.SetBool("rolling", true);

            // Start the coroutine to handle the roll duration
            StartCoroutine(WaitForAnimation());
        }
    }
    
    public bool IsMoving()
    {
        return movement.magnitude >= 0.1f;
    }
    
    void OnControllerColliderHit(ControllerColliderHit other)
    {
        // Check if the character is grounded based on collision with a "Ground" tagged object
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            velocity.y = 0; // Reset vertical velocity when grounded
            playerAnim.SetBool("falling", false);
        }
    }
    
    IEnumerator WaitForAnimation()
    {
        // Get the Animator State Info of the current animation
        AnimatorStateInfo animationInfo = playerAnim.GetCurrentAnimatorStateInfo(0);

        // Wait for the length of the animation
        yield return new WaitForSeconds(animationInfo.length);

        if (isAttacking)
        {
            // After the animation is over, reset the isAttacking flag and animation state
            isAttacking = false;
            playerAnim.SetBool("attack2H", false);
        }
        if (isRolling)
        {
            isRolling = false;
            playerAnim.SetBool("rolling", false);
        }
    }
}