using NUnit.Framework;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Stamina / Fatigue")]
    public float maxStamina = 100f;
    public float currentStamina = 100f;
    public float jumpCost = 20f;
    public float regenRate = 15f; // stamina per second when not jumping
    public float fatigueJumpMultiplier = 0.5f; // reduce jump when tired
    public UnityEngine.UI.Slider staminaBar;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 7f;
    public float conveyorPush = 0f;


    [Header("Ground Check")]
    public LayerMask groundMask;
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.25f;


    private Rigidbody rb;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Stop player from tipping over
        rb.linearDamping = 0.25f;



        currentStamina = maxStamina;
        if (staminaBar != null)
            staminaBar.maxValue = maxStamina;


    }

    void Update()
    {

        UpdateGrounded();
        HandleMovement();
        HandleJump();
        LockZAxis();
        UpdateStamina();


    }
    void UpdateStamina()
    {
        // REGENERATE STAMINA WHEN NOT JUMPING
        if (isGrounded && !Input.GetKey(KeyCode.Space))
        {
            currentStamina += regenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }

        // Update UI
        if (staminaBar != null)
            staminaBar.value = currentStamina;
    }

    void UpdateGrounded()
    {
        // Only the Ground layer counts (belt, floor)
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundMask);

    }

    void HandleMovement()
    {
        if (rb == null) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        Vector3 v = rb.linearVelocity;

        if (isGrounded)
        {
            // BASE VELOCITY: belt motion (only!)
            float baseSpeed = conveyorPush;

            // PLAYER INPUT ADDS OR SUBTRACTS
            float inputSpeed = moveX * moveSpeed;

            // FINAL:
            // player input ALWAYS overrides belt when opposite
            if (moveX != 0)
            {
                // If going opposite direction, ignore belt entirely
                if (Mathf.Sign(moveX) != Mathf.Sign(conveyorPush))
                    v.x = inputSpeed;
                else
                    v.x = baseSpeed + inputSpeed;
            }
            else
            {
                // No input → belt carries player
                v.x = baseSpeed;
            }
        }
        else
        {
            // AIR — limited control (don’t fight belt completely)
            float airTarget = moveX * (moveSpeed * 0.4f);
            v.x = Mathf.Lerp(v.x, airTarget + conveyorPush * 0.3f, 0.08f);
        }

        rb.linearVelocity = v;
    }





    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Not enough stamina → weak or no jump
            if (currentStamina <= 0)
            {
                // TOO TIRED → cannot jump
                return;
            }

            float jumpStrength = jumpForce;

            // Low stamina → weaker jump
            if (currentStamina < jumpCost)
            {
                jumpStrength *= fatigueJumpMultiplier;
            }

            // Consume stamina
            currentStamina -= jumpCost;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

            // Reset vertical velocity before jump
            Vector3 vel = rb.linearVelocity;
            vel.y = 0;
            rb.linearVelocity = vel;

            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);

            // Sound
            AudioManager.Instance.PlayJump();
        }
    }


    void LockZAxis()
    {
        if (Mathf.Abs(transform.position.z) > 0.001f)
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void OnDrawGizmosSelected()
    {
        // To visualize ground check
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}
