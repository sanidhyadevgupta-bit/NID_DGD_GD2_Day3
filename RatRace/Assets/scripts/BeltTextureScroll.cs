using UnityEngine;

public class BeltTextureScroll : MonoBehaviour
{
    public float scrollSpeed = 0.3f;
    public ConveyorBelt belt; // drag reference

    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed * belt.direction;
        rend.material.mainTextureOffset = new Vector2(offset, 0);
    }
}
