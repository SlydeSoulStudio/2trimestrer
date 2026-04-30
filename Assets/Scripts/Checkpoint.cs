using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            // Guardar checkpoint en GameLivesManager
            if (GameLivesManager.Instance != null)
                GameLivesManager.Instance.SetCheckpoint(transform);

            // Desactivar checkpoint visual
            gameObject.SetActive(false);
        }
    }
}
