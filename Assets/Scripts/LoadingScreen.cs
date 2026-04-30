using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{

    public Slider slider;

    void Start()
    {
        StartCoroutine(LoadAsync());
    }


    IEnumerator LoadAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(MenuScript.sceneToLoad);
        while (!operation.isDone)
        {
            slider.value = operation.progress;
            yield return null;
        }
    }
}
