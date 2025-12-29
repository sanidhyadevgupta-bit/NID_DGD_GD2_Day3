using UnityEngine;
using System.Collections.Generic;

public class ConveyorBelt_XOnly : MonoBehaviour
{
    [Header("Conveyor Settings")]
    public float beltSpeed = 5f;
    public float changeInterval = 2f;

    [Header("Texture Settings")]
    public Renderer beltRenderer;
    public float textureScrollMultiplier = 0.25f;

    float timer;
    float currentDirection = 1f; // +1 = right, -1 = left

    // Track rigidbodies that touch the belt
    readonly HashSet<Rigidbody> tracked = new HashSet<Rigidbody>();

    void FixedUpdate()
    {
        // Change direction ONLY on X axis
        timer += Time.fixedDeltaTime;
        if (timer >= changeInterval)
        {
            timer = 0f;
            currentDirection = (Random.value < 0.5f) ? 1f : -1f; // Flip sign
        }

        // Move objects along X only
        foreach (var rb in tracked)
        {
            if (rb != null && !rb.isKinematic)
            {
                rb.position += Vector3.right * currentDirection * beltSpeed * Time.fixedDeltaTime;
            }
        }

        // Texture scroll (U axis only)
        if (beltRenderer && beltRenderer.sharedMaterial)
        {
            Vector2 uv = beltRenderer.sharedMaterial.mainTextureOffset;
            uv.x += currentDirection * beltSpeed * textureScrollMultiplier * Time.fixedDeltaTime;
            beltRenderer.sharedMaterial.mainTextureOffset = uv;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Rigidbody rb = col.rigidbody;
        if (rb != null && !rb.isKinematic)
            tracked.Add(rb);
    }

    void OnCollisionExit(Collision col)
    {
        Rigidbody rb = col.rigidbody;
        if (rb != null)
            tracked.Remove(rb);
    }
}
