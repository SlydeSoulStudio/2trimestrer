using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData dialogue;
    bool playerInRange = false;

    TalkIconController iconController;

    void Start()
    {
        iconController = GetComponent<TalkIconController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            iconController?.ShowIcon();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            iconController?.HideIcon();
        }
    }

    void Update()
    {
        // si hay diálogo abierto, ocultar icono
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueOpen())
        {
            iconController?.HideIcon();
            return;
        }

        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            iconController?.HideIcon();
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
}
