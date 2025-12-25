using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpBoost = 8f;
    private Rigidbody rb;
    private SpringJoint spring;
    private bool canJump = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CreateSpring();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpBoost, ForceMode.Impulse);
        Destroy(spring);
        spring = null;
        canJump = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
            if (spring == null)
                CreateSpring();
        }
    }

    void CreateSpring()
    {
        spring = gameObject.AddComponent<SpringJoint>();
        spring.autoConfigureConnectedAnchor = false;
        spring.anchor = new Vector3(0, -0.9f, 0);
        spring.connectedAnchor = Vector3.zero;

        spring.spring = 300;
        spring.damper = 50;
        spring.minDistance = 0;
        spring.maxDistance = 0.5f; 
        spring.enableCollision = true;
        spring.breakForce = Mathf.Infinity;
        spring.breakTorque = Mathf.Infinity;
    }
}
