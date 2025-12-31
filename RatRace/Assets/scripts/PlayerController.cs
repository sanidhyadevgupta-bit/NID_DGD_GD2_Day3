using NUnit.Framework;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
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

      
    }

    void Update()
    {
      
        UpdateGrounded();
        HandleMovement();
        HandleJump();
        LockZAxis();
    }

    void UpdateGrounded()
    {
        // Only the Ground layer counts (belt, floor)
        isGrounded = Physics.CheckSphere(groundCheckPoint.position,groundCheckRadius,groundMask);
       
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
            Vector3 vel = rb.linearVelocity;
            vel.y = 0; 
            rb.linearVelocity = vel;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
