using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 20;
    public AudioClip pickupSound;
    public GameObject pickupParticles;

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            // curar al jugador
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.RecoverHealth(healAmount);

            //reproducir sonido desde el jugador
            AudioSource playerAudio = other.GetComponent<AudioSource>();
            if (playerAudio != null && pickupSound != null)
                playerAudio.PlayOneShot(pickupSound);

            //particulas al desaparecer
            if (pickupParticles != null)
                Instantiate(pickupParticles, transform.position, Quaternion.identity);

            // destruir pickup
            Destroy(gameObject);
        }
    }
}