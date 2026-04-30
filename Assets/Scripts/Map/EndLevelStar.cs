using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelStar : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip starSound;
    public GameObject starEffect;      //efecto visual
    public string sceneToLoad = "EndLevel1";

    bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        // Instanciar efecto visual
        if (starEffect != null)
            Instantiate(starEffect, transform.position, Quaternion.identity);

        // Reproducir sonido
        if (audioSource != null && starSound != null)
            audioSource.PlayOneShot(starSound);

        // Esperar al sonido y cargar escena
        float delay = (starSound != null) ? starSound.length : 0f;
        Invoke(nameof(LoadScene), delay);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
