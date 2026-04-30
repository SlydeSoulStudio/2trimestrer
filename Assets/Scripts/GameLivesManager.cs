using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLivesManager : MonoBehaviour
{
    public static GameLivesManager Instance;

    public int lives = 3;

    private Transform checkpoint;
    private bool hasCheckpoint = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetCheckpoint(Transform cp)
    {
        checkpoint = cp;
        hasCheckpoint = true;
    }

    public void RespawnPlayer()
    {
        lives--;

        // Actualizar UI
        FindFirstObjectByType<LivesUI>()?.UpdateLives();

        if (lives > 0)
        {
            RespawnAtCheckpoint();
        }
        else
        {
            RestartLevel();
        }
    }

    private void RespawnAtCheckpoint()
    {
        PlayerHealth player = FindFirstObjectByType<PlayerHealth>();

        if (player != null)
        {
            if (hasCheckpoint)
                player.ResetAfterRespawn(checkpoint.position);
            else
                RestartLevel();
        }
    }

    public void RestartLevel()
    {
        lives = 3;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
