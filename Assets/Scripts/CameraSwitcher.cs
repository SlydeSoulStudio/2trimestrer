using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineCamera camMain;
    public CinemachineCamera camClose;
    public CinemachineCamera camTop;

    public PlayerMovement playerMovement;
    public AudioSource audioSource;
    public AudioClip cinematicSound;
    public GameObject cinematicEffect;

    public float cinematicDuration = 4f;

    void Update()
    {
        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            PlayCinematic(camTop, cinematicDuration);
        }
    }

    public void SetCamera(CinemachineCamera cam)
    {
        camMain.Priority = 0;
        camClose.Priority = 0;
        camTop.Priority = 0;
        cam.Priority = 10;
    }

    public void PlayCinematic(CinemachineCamera cam, float duration)
    {
        // 1. Activar cámara cinemática
        SetCamera(cam);

        // 2. Bloquear movimiento
        if (playerMovement != null)
            playerMovement.movementLocked = true;

        // 3. Sonido
        if (audioSource && cinematicSound)
            audioSource.PlayOneShot(cinematicSound);

        // 4. Efecto
        if (cinematicEffect)
            cinematicEffect.SetActive(true);

        // 5. Volver a la cámara principal después de X segundos
        CancelInvoke(nameof(ReturnToMainCamera));
        Invoke(nameof(ReturnToMainCamera), duration);
    }

    void ReturnToMainCamera()
    {
        // volver a la cámara principal
        SetCamera(camMain);

        // desbloquear movimiento
        if (playerMovement != null)
            playerMovement.movementLocked = false;

        // desactivar efecto
        if (cinematicEffect)
            cinematicEffect.SetActive(false);
    }
}
