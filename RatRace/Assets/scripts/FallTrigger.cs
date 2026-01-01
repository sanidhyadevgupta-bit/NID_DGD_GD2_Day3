using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    public enum Side { Left, Right }
    public Side side;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Debug.Log($"Trigger hit by {other.name} (IGNORED)");
            return;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is NULL — check that GameManager exists in scene.");
            return;
        }

        if (side == Side.Left)
        {
            Debug.Log("LEFT TRIGGER ACTIVATED");
            GameManager.Instance.GameOver();
        }
        else
        {
            Debug.Log("RIGHT TRIGGER ACTIVATED");
            GameManager.Instance.GameOver();
        }
    }
}
