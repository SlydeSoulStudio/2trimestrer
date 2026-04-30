using UnityEngine;
using TMPro;

public class EndLevelScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        scoreText.text = "Score: " + finalScore;
    }
}
