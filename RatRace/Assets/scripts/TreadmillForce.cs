using UnityEngine;

public class TreadmillForce : MonoBehaviour
{
    public float conveyorForce = 50f; // must be slightly > player runForce

    void OnCollisionStay(Collision col)
    {
        Rigidbody rb = col.rigidbody;
        if (!rb) return;

        // Push player LEFT as long as they are touching
        rb.AddForce(Vector3.left * conveyorForce, ForceMode.Force);
    }
}
