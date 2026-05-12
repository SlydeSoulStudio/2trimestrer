using UnityEngine;
using TMPro;

public class EndLevelScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI A1scoreText;
    public TextMeshProUGUI A2scoreText;
    public TextMeshProUGUI A3scoreText;

    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        scoreText.text = "Score: " + finalScore;

        A1scoreText.text = "Area 1: " + GameData.a1;
        A1scoreText.text = "Area 2: " + GameData.a2;
        A1scoreText.text = "Area 3: " + GameData.a3;
    }
}
