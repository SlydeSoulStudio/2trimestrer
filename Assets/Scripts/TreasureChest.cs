using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TreasureChest : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactUI;

    [Header("Treasure Settings")]
    public int storedTreasures = 0;
    public int requiredTreasures = 5;

    [Header("Audio")]
    public AudioSource depositSound; //sonido al depositar

    // Evento para actualizar la UI del cofre
    public Action<int, int> OnChestChanged;

    private bool playerNearby = false;
    private TreasureInventory inventory;

    void Start()
    {
        interactUI.SetActive(false);

        // Actualizar UI al inicio
        OnChestChanged?.Invoke(storedTreasures, requiredTreasures);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventory = other.GetComponent<TreasureInventory>();
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
            int deposited = inventory.DepositAll();

            if (deposited > 0)
            {
                storedTreasures += deposited;

                //reproducir sonido solo si realmente depositas algo
                if (depositSound != null)
                    depositSound.Play();

                //actualizar UI
                OnChestChanged?.Invoke(storedTreasures, requiredTreasures);

                //comprobar si el juego está completado
                if (storedTreasures >= requiredTreasures)
                {

                    // pantalla final
                    SceneManager.LoadScene("Mission1End");
                }
            }
        }
    }
}