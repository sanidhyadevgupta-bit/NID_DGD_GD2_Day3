using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public float score = 0f;

    void Update()
    {
        score += Time.deltaTime;
        scoreText.text = "Distance: " + Mathf.FloorToInt(score).ToString();
    }
}
