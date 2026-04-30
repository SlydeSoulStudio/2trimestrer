using UnityEngine;
using TMPro;

public class LivesUI : MonoBehaviour
{
    public TextMeshProUGUI livesText;

    void Start()
    {
        UpdateLives();
    }

    public void UpdateLives()
    {
        if (livesText != null)
            livesText.text = "Vidas: " + GameLivesManager.Instance.lives;
    }
}