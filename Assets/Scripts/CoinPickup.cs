using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinAmount = 1;
    public AudioClip pickupSound;
    public GameObject pickupParticles;

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            //sumar monedas al GameData
            GameData.coins += coinAmount;

            //reproducir sonido desde el jugador
            AudioSource playerAudio = other.GetComponent<AudioSource>();
            if (playerAudio != null && pickupSound != null)
                playerAudio.PlayOneShot(pickupSound);

            // ⭐ partículas al recoger
            if (pickupParticles != null)
                Instantiate(pickupParticles, transform.position, Quaternion.identity);

            //destruir moneda
            Destroy(gameObject);
        }
    }
}
