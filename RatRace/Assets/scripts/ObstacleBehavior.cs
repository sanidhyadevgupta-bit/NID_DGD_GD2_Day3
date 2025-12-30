using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.mass = 1.2f; // tweakable for weighty feeling
        rb.linearDamping = 0.5f;
        rb.angularDamping = 1f;
    }

    // Prevent excessive velocity
    void FixedUpdate()
    {
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, 20f);
    }
}
