using UnityEngine;
using UnityEngine.InputSystem;

public class TreasurePickup : MonoBehaviour
{
    public GameObject interactUI;
    public AudioSource pickupSound;

    private bool playerNearby = false;
    private TreasureInventory inventory;
    private PlayerAudio playerAudio;

    void Start()
    {
        interactUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventory = other.GetComponent<TreasureInventory>();
            playerAudio = other.GetComponent<PlayerAudio>();

            playerNearby = true;
            interactUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            interactUI.SetActive(false);
        }
    }

    void Update()
    {
        if (playerNearby && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (inventory.CanCarryMore())
            {
                interactUI.SetActive(false);

                //reproducir sonido desde el jugador
                if (playerAudio != null && pickupSound != null)
                    playerAudio.PlayPickupSound(pickupSound.clip);

                inventory.AddTreasure();

                //destruir el tesoro instantáneamente
                Destroy(gameObject);
            }
        }
    }
}