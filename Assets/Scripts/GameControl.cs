using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject inGame;
    public GameObject gameOver;
    bool isPaused;
    bool playerAlive = true;

    void Start()
    {
        Cursor.visible = false;                     //oculta el cursor al iniciar
        Cursor.lockState = CursorLockMode.Locked;   //bloquea el cursor al iniciar

        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (playerAlive)
            {
                TogglePause();
            }
        }
    }

    public void TogglePause()
    {
        if (isPaused == true)
        {
            pauseMenu.SetActive(false);
            inGame.SetActive(true);
            Time.timeScale = 1.0f;
            isPaused = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            pauseMenu.SetActive(true);
            inGame.SetActive(false);
            Time.timeScale = 0f;
            isPaused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ContinueGame()
    {
        pauseMenu.SetActive(false);
        inGame.SetActive(true);
        Time.timeScale = 1.0f;
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //this prevent the resume to stay after close
        EventSystem.current.SetSelectedGameObject(null);

    }


    public void RestartLevel(float delay)
    {
        playerAlive = false;
        StartCoroutine(ReloadAfterTime(delay));
    }

    IEnumerator ReloadAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowGameOver();
    }

    public void ShowGameOver()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gameOver.SetActive(true);
        inGame.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void TryAgain()
    {
        MenuScript.sceneToLoad = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("LoadingScreen");
    }
}
