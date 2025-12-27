using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Forces")]
    public float runForce = 40f;
    public float maxRightSpeed = 3f;
    public float jumpForce = 14f;

    [Header("Boop Settings")]
    public float boopLeftForce = -10f;
    public float boopDownForce = -8f;
    public float boopStunTime = 0.35f;

    [Header("Fail State")]
    public float fallLimit = -5f;

    Rigidbody rb;
    bool grounded = false;
    bool stunned = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;  // prevent tipping over
    }

    void Update()
    {
        // Attempt to jump only when grounded & not stunned
        if (Input.GetKeyDown(KeyCode.Space) && grounded && !stunned)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            grounded = false;
        }

        CheckFallDeath();
        if (Input.GetKeyDown(KeyCode.Space) && !grounded)
    Debug.Log("Blocked: Not Grounded");

    }
    

    void FixedUpdate()
    {
        if (!stunned)
            ApplyRunForce();

        ClampRunSpeed();
    }

    // -----------------------------------
    // Movement Logic
    // -----------------------------------
    void ApplyRunForce()
    {
        rb.AddForce(Vector3.right * runForce, ForceMode.Force);
    }

    void ClampRunSpeed()
    {
        Vector3 v = rb.linearVelocity;
        if (v.x > maxRightSpeed)
            v.x = maxRightSpeed;
        rb.linearVelocity = v;
    }

    // -----------------------------------
    // Ground Detection via Collision
    // -----------------------------------
    void OnCollisionStay(Collision col)
    {
        // Ground layer = treadmill surface
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            grounded = true;
    }

    void OnCollisionExit(Collision col)
    {
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            grounded = false;
    }

    // -----------------------------------
    // Boop Collision (Obstacle)
    // -----------------------------------
    void OnCollisionEnter(Collision col)
    {
        if (!col.collider.CompareTag("Obstacle")) return;

        // Ignore if rising fast enough (clean jump through)
        if (rb.linearVelocity.y > jumpForce * 0.4f) return;

        // Apply knockback
        stunned = true;
        grounded = false;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(new Vector3(boopLeftForce, boopDownForce, 0), ForceMode.Impulse);

        Invoke(nameof(Recover), boopStunTime);
    }

    void Recover()
    {
        stunned = false;
    }

    // -----------------------------------
    // Fail State
    // -----------------------------------
    void CheckFallDeath()
    {
        if (transform.position.y < fallLimit)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // -----------------------------------
    // Debug Gizmo (Ground status)
    // -----------------------------------
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = grounded ? Color.green : Color.red;
        Gizmos.DrawLine(
            transform.position + Vector3.down * 0.1f,
            transform.position + Vector3.down * 0.45f
        );
        Gizmos.DrawSphere(transform.position + Vector3.down * 0.45f, 0.05f);
    }
}
