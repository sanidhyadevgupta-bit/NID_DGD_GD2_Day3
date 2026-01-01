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
        if (timer >= changeInterval) // direction change 
        {
            direction *= -1;
            timer = 0f;
            // Trigger lamp
            WarningLight lamp = FindFirstObjectByType<WarningLight>();
            if (lamp != null){
                lamp.FlashDirection(direction);
                Debug.Log("Flashing");
        }
        }
    }

    void FixedUpdate()
    {
        Vector3 beltVel = Vector3.right * direction * beltSpeed;

        foreach (Rigidbody rb in bodiesOnBelt)
        {
            if (rb == null) continue;

            PlayerController pc = rb.GetComponent<PlayerController>();

            // PLAYER LOGIC (belt influence but not override)
            if (pc != null)
            {
                // Let player script handle movement
                pc.conveyorPush = beltVel.x;
                continue;
            }

            // OBSTACLES & OTHER RIGIDBODIES
            // Directly move them with the belt
            Vector3 v = rb.linearVelocity;
            v.x = beltVel.x;
            rb.linearVelocity = v;
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
