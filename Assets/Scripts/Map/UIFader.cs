using UnityEngine;

public class UIFader : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;

    void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        // Garantiza que empieza invisible sin desactivar el objeto
        canvasGroup.alpha = 0f;
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(1f));
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(0f));
    }

    private System.Collections.IEnumerator FadeRoutine(float target)
    {
        float start = canvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, target, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = target;
    }
}
