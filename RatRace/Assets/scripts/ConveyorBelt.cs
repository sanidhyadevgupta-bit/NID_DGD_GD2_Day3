using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ConveyorBelt : MonoBehaviour
{
    [Header("Settings")]
    public float beltSpeed = 5f;
    public float direction = 1f; // 1 = right, -1 = left
    public float changeInterval = 3f;

    float timer;
    List<Rigidbody> bodiesOnBelt = new();

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= changeInterval)
        {
            direction *= -1;
            timer = 0f;
        }
    }

    void FixedUpdate()
    {
        Vector3 beltVel = Vector3.right * direction * beltSpeed;

        foreach (Rigidbody rb in bodiesOnBelt)
        {
            if (rb == null) continue;

            // Core movement logic (RELIABLE)
            Vector3 v = rb.linearVelocity;
            v.x = beltVel.x;                 // conveyor sets horizontal motion
            rb.linearVelocity = v;

            // Player support
            PlayerController pc = rb.GetComponent<PlayerController>();
            if (pc)
                pc.conveyorPush = beltVel.x; // Player knows which way belt is going
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null && !bodiesOnBelt.Contains(rb))
            bodiesOnBelt.Add(rb);
    }

    void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null && bodiesOnBelt.Contains(rb))
        {
            bodiesOnBelt.Remove(rb);

            // player leaves belt → no influence
            PlayerController pc = rb.GetComponent<PlayerController>();
            if (pc) pc.conveyorPush = 0f;
        }
    }
}
