using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BrandScreen : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1.5f;
    public float waitTime = 1f;

    void Start()
    {
        Screen.SetResolution(1920, 1080, false);
        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        // FADE IN
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        // Espera con el logo visible
        yield return new WaitForSeconds(waitTime);

        // FADE OUT
        t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        // Cargar menú
        SceneManager.LoadScene("MainMenu");
    }
}
