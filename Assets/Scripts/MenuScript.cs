using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public static string sceneToLoad;

    void Start()
    {
        //Always the cursor in the menu
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Play()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        sceneToLoad = "Intro";
        SceneManager.LoadScene("LoadingScreen");
    }

    public void StartLevel1()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        sceneToLoad = "Level1";
        SceneManager.LoadScene("LoadingScreen");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        sceneToLoad = "MainMenu";
        SceneManager.LoadScene("LoadingScreen");
    }


}