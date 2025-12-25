using UnityEngine;

public class Treadmill : MonoBehaviour
{
    public float conveyorSpeed = 2.5f; // recommended start speed

    void OnCollisionStay(Collision col)
    {
        Rigidbody rb = col.rigidbody;
        if (!rb) return;

        PlayerState state = rb.GetComponent<PlayerState>();

        // Do NOT drag harder during recovery
        if (state != null && state.isRecovering) return;

        // Pull player LEFT (primary gameplay pressure)
        rb.AddForce(Vector3.left * conveyorSpeed, ForceMode.Acceleration);
    }
}
