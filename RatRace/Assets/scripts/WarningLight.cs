using UnityEngine;

public class WarningLight : MonoBehaviour
{
    public MeshRenderer lampRenderer;
    public Light lampLight;

    [Header("Colors")]
    public Color rightColor = Color.green;
    public Color leftColor = Color.red;

    [Header("Flash")]
    public float flashTime = 0.6f;
    public float maxIntensity = 4f;
    public float scalePunch = 1.4f;

    float timer;
    Vector3 baseScale;
    Color baseColor;
    float baseLightIntensity;

    void Start()
    {
        if (!lampRenderer)
            lampRenderer = GetComponent<MeshRenderer>();

        baseScale = transform.localScale;
        baseColor = lampRenderer.material.color;

        if (lampLight)
            baseLightIntensity = lampLight.intensity;
    }

    public void FlashDirection(float direction)
    {
        Color c = direction > 0 ? rightColor : leftColor;

        lampRenderer.material.color = c;

        if (lampLight)
        {
            lampLight.color = c;
            lampLight.intensity = maxIntensity;
        }

        transform.localScale = baseScale * scalePunch;
        timer = flashTime;
    }

    void Update()
    {
        if (timer <= 0f) return;

        timer -= Time.deltaTime;

        float t = timer / flashTime;

        // Smoothly return to normal
        lampRenderer.material.color = Color.Lerp(baseColor, lampRenderer.material.color, t);
        transform.localScale = Vector3.Lerp(baseScale, transform.localScale, t);

        if (lampLight)
            lampLight.intensity = Mathf.Lerp(baseLightIntensity, lampLight.intensity, t);
    }
}
