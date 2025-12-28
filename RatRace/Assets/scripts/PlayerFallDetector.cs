using UnityEngine;

public class PlayerFallDetector : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftFall"))
        {
            GameManager.Instance.GameOverLeft();
        }

        if (other.CompareTag("RightFall"))
        {
            GameManager.Instance.GameOverRight();
        }
    }
}
