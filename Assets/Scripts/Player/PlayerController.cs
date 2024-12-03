using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Player Basic Parameters")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float rollingSpeed = 10f;
    public float rotationSpeed = 720F;
    public float gravityScale = 2f; // Scales gravity when falling.
    public bool isAttacking = false;
    private CharacterController controller;
    private Transform cameraTransform;
    private Vector3 movement;
    private Vector3 velocity;
    private float currentSpeed;

    [SerializeField] private bool isGrounded;

    // Jump parameters.
    [SerializeField] private float baseGravity = -9.81f;
    private float currentGravity;
    [SerializeField] private float jumpForce;
    public bool isJumping;
    public bool isRolling;

    [Header ("Animation")]
    public Animator playerAnim;


    //---------------------------------------------------------------------------//

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
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

        // Apply movement and gravity.
        controller.Move((movement * moveSpeed + velocity) * Time.deltaTime);
    }

    //--------------------------------------------------------------------------------//

    void Movement()
    {
        // Get Inputs.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Calculate movement vector.
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        movement = (forward * vertical + right * horizontal).normalized;

        // Sprint feature
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isSprinting ? sprintSpeed : moveSpeed; // If isSprinting = true, it will use sprintSpeed if not moveSpeed.
        currentSpeed = isRolling ? rollingSpeed : moveSpeed; // If isRolling = true, it will use rollingSpeed if not moveSpeed.

        // Apply movement.
        if (movement.magnitude >= 0.1f && isGrounded)
        {
            // Rotate the character to face the movement direction.
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move the character.
            controller.Move(movement * currentSpeed * Time.deltaTime);

            // Play movement animation.
            playerAnim.SetBool("running", true);
        }
        else
        {
            playerAnim.SetBool("running", false);
        }


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

            // Apply higher gravity when falling.
            if (velocity.y < 0)
            {
                currentGravity = baseGravity * gravityScale;
            }
            else
            {
                currentGravity = baseGravity; // Normal gravity when jumping upwards.
            }
        }
        else
        {
            currentGravity = baseGravity; // Normal gravity when grounded.
        }

        velocity.y += currentGravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime); // Apply gravity.
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
        // Check if the character is grounded based on collision with a "Ground" tagged object.
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            velocity.y = 0; // Reset vertical velocity when grounded

            playerAnim.SetBool("falling", false);
        }
    }
    
    IEnumerator WaitForAnimation()
    {
        // Get the Animator State Info of the current animation (assuming it's in the "Base Layer")
        AnimatorStateInfo animationInfo = playerAnim.GetCurrentAnimatorStateInfo(0);

        // Wait for the length of the attack animation
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
