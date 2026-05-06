using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelStar : MonoBehaviour
{
    public AudioClip starSound;
    public GameObject starEffect;      // efecto visual
    public int effectCount = 3;        // cuántos efectos spawnear
    public float effectRadius = 0.5f;  // dispersión de los efectos
    public string sceneToLoad = "EndLevel1";

    bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        // --- REPRODUCIR SONIDO DESDE EL JUGADOR ---
        AudioSource playerAudio = other.GetComponent<AudioSource>();
        if (playerAudio != null && starSound != null)
            playerAudio.PlayOneShot(starSound);

        // --- SPAWN DE VARIOS EFECTOS ---
        if (starEffect != null)
        {
            for (int i = 0; i < effectCount; i++)
            {
                Vector3 randomPos = transform.position +
                    new Vector3(
                        Random.Range(-effectRadius, effectRadius),
                        Random.Range(0f, effectRadius),
                        Random.Range(-effectRadius, effectRadius)
                    );

                Instantiate(starEffect, randomPos, Quaternion.identity);
            }
        }

        // Esperar al sonido y cargar escena
        float delay = (starSound != null) ? starSound.length : 0f;
        Invoke(nameof(LoadScene), delay);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
