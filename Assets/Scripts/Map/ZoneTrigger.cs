using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class ZoneTrigger : MonoBehaviour
{
    [Header("UI")]
    public string zoneName;
    public TextMeshProUGUI zoneText;
    public UIFader fader;

    [Header("Cinematic")]
    public CameraSwitcher cameraSwitcher;
    public CinemachineCamera cinematicCamera;
    public float cinematicDuration = 4f;
    public PlayableDirector timeline;

    [Header("Player")]
    public PlayerMovement playerMovement;

    [Header("Audio & FX")]
    public AudioSource audioSource;
    public AudioClip zoneSound;
    public GameObject zoneEffect;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (triggered) return;
        triggered = true;

        // 1. Mostrar cartel
        zoneText.text = zoneName;
        fader.FadeIn();

        // 2. Sonido
        if (audioSource && zoneSound)
            audioSource.PlayOneShot(zoneSound);

        // 3. Efecto
        if (zoneEffect)
            zoneEffect.SetActive(true);

        // 4. Bloquear movimiento
        if (playerMovement != null)
            playerMovement.movementLocked = true;

        // 5. Activar cámara cinemática
        cameraSwitcher.SetCamera(cinematicCamera);

        // 6. Reproducir Timeline
        if (timeline != null)
        {
            timeline.time = 0;
            timeline.Play();
        }

        // 7. Ocultar cartel
        Invoke(nameof(HideTitle), 2f);

        // 8. Volver al jugador
        Invoke(nameof(RestoreMovement), cinematicDuration);
    }

    void HideTitle()
    {
        fader.FadeOut();
    }

    void RestoreMovement()
    {
        if (playerMovement != null)
            playerMovement.movementLocked = false;

        if (zoneEffect)
            zoneEffect.SetActive(false);

        // volver a la cámara principal
        cameraSwitcher.SetCamera(cameraSwitcher.camMain);
    }
}
