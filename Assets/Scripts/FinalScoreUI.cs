using UnityEngine;
using TMPro;

public class FinalScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        Debug.Log("VALOR ENTRANDO A LA ESCENA FINAL: " + GameData.coins);
        scoreText.text = "Final Score: " + GameData.coins;
    }
}
