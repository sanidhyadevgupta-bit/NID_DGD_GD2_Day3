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
        isGrounded = Physics.CheckSphere(
            groundCheckPoint.position,
            groundCheckRadius,
            groundMask
        );
    }

void HandleMovement()
{
    if (rb == null) return; // Safety so no null errors

    float moveX = Input.GetAxisRaw("Horizontal");
    Vector3 v = rb.linearVelocity;

    // GROUND MOVEMENT
    if (isGrounded)
    {
        // Player controls first — belt influence second
        v.x = moveX * moveSpeed;

        // Only add conveyor influence if any exists
        if (Mathf.Abs(conveyorPush) > 0.01f)
            v.x += conveyorPush * 0.6f;
    }
    else // AIR MOVEMENT
    {
        // Limited air control
        v.x = Mathf.Lerp(v.x, moveX * (moveSpeed * 0.4f), 0.08f);

        // Keep from drifting too much
        v.x = Mathf.Clamp(v.x, -moveSpeed, moveSpeed);
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
