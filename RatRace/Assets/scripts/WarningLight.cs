using UnityEngine;

public class WarningLight : MonoBehaviour
{
    public MeshRenderer lampRenderer;
    public Color rightColor = Color.green;
    public Color leftColor = Color.red;
    public float flashTime = 0.4f;

    private float timer = 0f;
    private bool flashing = false;
    private Color defaultColor;

    void Start()
    {
        if (lampRenderer == null)
            lampRenderer = GetComponent<MeshRenderer>();

        defaultColor = lampRenderer.material.color;
    }

    public void FlashDirection(float direction)
    {
        if (direction > 0)
            lampRenderer.material.color = rightColor;
           
        else
            lampRenderer.material.color = leftColor;

        flashing = true;
        timer = flashTime;
    }

    void Update()
    {
        if (!flashing) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            lampRenderer.material.color = defaultColor;
            flashing = false;
        }
    }
}
