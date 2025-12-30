using UnityEngine;

public class ImageSequenceDisplay : MonoBehaviour
{
    [Header("Assign Planes (order matters)")]
    public Renderer[] planes;              // 6 planes in order

    [Header("Assign Textures (must match plane count or exceed)")]
    public Texture[] images;               // images to cycle through 1-by-1

    public float interval = 30f;           // 30 seconds per change
    private int currentIndex = 0;
    private float timer = 0f;

    void Start()
    {
        if (planes.Length == 0)
        {
            Debug.LogError("No planes assigned!");
            enabled = false;
            return;
        }

        // Initialize first plane texture if possible
        ApplyImageToPlane(0);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            currentIndex++;

            if (currentIndex < planes.Length)
                ApplyImageToPlane(currentIndex);
            else
            {
                // Optional: Stop when all planes are filled
                Debug.Log("All planes updated.");
                enabled = false;
            }
        }
    }

    void ApplyImageToPlane(int index)
    {
        if (index < planes.Length && index < images.Length)
        {
            planes[index].material.mainTexture = images[index];
        }
        else
        {
            Debug.LogWarning("Plane or Image index out of range for index: " + index);
        }
    }
}
