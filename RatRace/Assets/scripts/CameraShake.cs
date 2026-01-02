using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    public float shakeDuration = 0.15f;
    public float shakeMagnitude = 0.15f;
    public float dampingSpeed = 20f;

    private float currentDuration = 0f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (currentDuration > 0)
        {
            transform.localPosition = originalPosition +
                Random.insideUnitSphere * shakeMagnitude;

            currentDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            currentDuration = 0f;
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                originalPosition,
                Time.deltaTime * dampingSpeed
            );
        }
    }

    public void Shake(float intensityMultiplier = 1f)
    {
        currentDuration = shakeDuration;
        shakeMagnitude *= intensityMultiplier;
    }
}
