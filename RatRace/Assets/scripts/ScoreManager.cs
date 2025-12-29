using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public float score = 0f;

    void Update()
    {
        score += Time.deltaTime;
     
        scoreText.text = "Time " + Mathf.FloorToInt(score).ToString()+"sec";
    }
}
