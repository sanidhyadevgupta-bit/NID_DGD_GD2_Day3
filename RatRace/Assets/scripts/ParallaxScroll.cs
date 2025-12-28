using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    [System.Serializable]
    public class Layer
    {
        public Renderer rend;
        public float speedMultiplier;
    }

    public Layer[] layers;
    public float baseScrollSpeed = 1f; // adjust to match treadmill feel

    void Update()
    {
        foreach (Layer layer in layers)
        {
            if (!layer.rend) continue;

            float offset = Time.time * baseScrollSpeed * layer.speedMultiplier;
            layer.rend.material.mainTextureOffset = new Vector2(offset, 0);
        }
    }
}
