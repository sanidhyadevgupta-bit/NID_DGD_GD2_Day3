using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpPower = 14f;

    [Header("Drift Control")]
    public float maxLeftSpeed = -3f;  // treadmill drag limit
    public float maxRightSpeed = 0f;  // no right movement allowed

    [Header("Hit Reaction")]
    public float boopUpPower = 5f;
    public float boopBackPower = 3f;
    public float collisionCooldown = 0.5f;

    [Header("Game Over")]
    public float fallDeathY = -5f; // if player falls below this -> game over

    private Rigidbody rb;
    private SpringJoint spring;
    private PlayerState state;
    private bool canJump = true;
    private float obstacleIgnoreTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        state = GetComponent<PlayerState>();

        CreateSpring();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // no weird flips
    }

    void Update()
    {
        // Pure vertical jump
        if (Input.GetKeyDown(KeyCode.Space) && canJump && !state.isRecovering)
            Jump();

        // Never move right, only left drift allowed
        Vector3 v = rb.velocity;
        v.x = Mathf.Clamp(v.x, maxLeftSpeed, maxRightSpeed);
        rb.velocity = v;

        // GAME OVER: Fell off the world
        if (transform.position.y < fallDeathY)
            GameOver();
    }

    // ----------------------------------------------------------
    // JUMP (Vertical Only)
    // ----------------------------------------------------------
    void Jump()
    {
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

        if (spring != null)
            Destroy(spring);

        canJump = false;
    }

    // ----------------------------------------------------------
    // COLLISIONS
    // ----------------------------------------------------------
    void OnCollisionEnter(Collision col)
    {
        // GROUND CONTACT
        if (col.gameObject.CompareTag("Ground") && !state.isRecovering)
        {
            if (spring == null)
                CreateSpring();
            canJump = true;
            return;
        }

        // IGNORE spam collisions
        if (Time.time < obstacleIgnoreTime)
            return;

        // OBSTACLE CONTACT
        if (col.gameObject.CompareTag("Obstacle"))
        {
            foreach (var contact in col.contacts)
            {
                // Player landed on TOP of obstacle
                if (contact.normal.y < -0.3f)
                {
                    ForceFallOffObstacle();
                    return;
                }
            }

            // SIDE HIT (knockback)
            SideHitBoop();
        }
    }

    // ----------------------------------------------------------
    // OBSTACLE TOUCHING (CONTINUOUS FALL-OFF)
    // ----------------------------------------------------------
    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.CompareTag("Obstacle"))
        {
            // Always fall off - no platforming allowed
            if (spring != null)
            {
                Destroy(spring);
                spring = null;
                canJump = false;
            }

            rb.AddForce(Vector3.down * 10f, ForceMode.Acceleration);
            rb.AddForce(Vector3.left * 2f, ForceMode.Acceleration);
        }
    }

    // ----------------------------------------------------------
    // TOP FALL-OFF RESPONSE
    // ----------------------------------------------------------
    void ForceFallOffObstacle()
    {
        if (spring != null)
        {
            Destroy(spring);
            spring = null;
        }

        canJump = false;

        rb.AddForce(Vector3.down * 12f, ForceMode.Impulse);
        rb.AddForce(Vector3.left * 3f, ForceMode.Impulse);

        transform.position += Vector3.left * 0.2f;
    }

    // ----------------------------------------------------------
    // SIDE HIT BOOP
    // ----------------------------------------------------------
    void SideHitBoop()
    {
        if (spring != null)
        {
            Destroy(spring);
            spring = null;
        }

        Vector3 boop = Vector3.up * boopUpPower + Vector3.left * boopBackPower;
        rb.AddForce(boop, ForceMode.Impulse);

        transform.position += Vector3.left * 0.15f;

        state.isRecovering = true;
        canJump = false;
        obstacleIgnoreTime = Time.time + collisionCooldown;
        StartCoroutine(RecoveryWindow());
    }

    // ----------------------------------------------------------
    // RECOVERY WINDOW
    // ----------------------------------------------------------
    IEnumerator RecoveryWindow()
    {
        yield return new WaitForSeconds(state.recoveryTime);

        if (IsGrounded() && spring == null)
            CreateSpring();

        state.isRecovering = false;
        canJump = IsGrounded();
    }

    // ----------------------------------------------------------
    // SPRING JOINT FOR STABILITY
    // ----------------------------------------------------------
    void CreateSpring()
    {
        spring = gameObject.AddComponent<SpringJoint>();
        spring.autoConfigureConnectedAnchor = true;
        spring.anchor = new Vector3(0, -0.9f, 0);
        spring.spring = 300;
        spring.damper = 40;
        spring.minDistance = 0;
        spring.maxDistance = 0.5f;
        spring.enableCollision = true;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    // ----------------------------------------------------------
    // GAME OVER
    // ----------------------------------------------------------
    void GameOver()
    {
        Debug.Log("GAME OVER: Player fell off the treadmill.");

        canJump = false;
        state.isRecovering = true;

        if (spring != null)
            Destroy(spring);

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        // Placeholder for respawn / retry UI
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
