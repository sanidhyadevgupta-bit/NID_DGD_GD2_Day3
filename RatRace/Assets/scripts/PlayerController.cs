using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 7f;

    private Rigidbody rb;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent tipping
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        LockZAxis();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector3(moveX * moveSpeed, rb.linearVelocity.y, 0);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Reset vertical velocity so jump is consistent
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // Immediately remove grounded state so no double jump
            isGrounded = false;
        }
    }

    // Only allow grounded if we are touching a collider beneath us
    void OnCollisionEnter(Collision col)
    {
        if (col.contacts.Length > 0)
        {
            ContactPoint c = col.contacts[0];

            // Ground only if the surface is below the player (prevents walls/ceilings)
            if (Vector3.Dot(c.normal, Vector3.up) > 0.5f)
                isGrounded = true;
        }
    }

    // If no longer colliding with anything, remove grounded state
    void OnCollisionExit(Collision col)
    {
        isGrounded = false;
    }

    void LockZAxis()
    {
        if (Mathf.Abs(transform.position.z) > 0.001f)
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
