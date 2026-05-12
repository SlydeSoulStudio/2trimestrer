using UnityEngine;
using TMPro;

public class FinalScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI A1scoreText;
    public TextMeshProUGUI A2scoreText;
    public TextMeshProUGUI A3scoreText;


    void Start()
    {
        Debug.Log("VALOR ENTRANDO A LA ESCENA FINAL: " + GameData.coins);
        scoreText.text = "Final Score: " + GameData.coins;

        A1scoreText.text = "Area 1: " + GameData.a1;
        A2scoreText.text = "Area 2: " + GameData.a2;
        A3scoreText.text = "Area 3: " + GameData.a3;
    }
}
