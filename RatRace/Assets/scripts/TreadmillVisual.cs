using UnityEngine;

public class TreadmillVisual : MonoBehaviour
{
    public Renderer rend;
    public float scrollSpeed = 1.5f;

    void Update()
    {
        if (!rend) return;
        float offset = Time.time * scrollSpeed;
        rend.material.SetTextureOffset("_BaseMap", new Vector2(-offset, 0));
    }
}
