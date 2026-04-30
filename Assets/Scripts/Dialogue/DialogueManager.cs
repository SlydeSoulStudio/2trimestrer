using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public GameObject continueButton;

    [Header("Typing Settings")]
    public float typingSpeed = 0.03f;

    [Header("Audio")]
    public AudioSource audioSource;      // sonido de la línea
    public AudioSource clickSource;      // sonido de click
    public AudioClip clickSound;         // clip de click

    DialogueData currentDialogue;
    int index = 0;
    bool isTyping = false;

    PlayerMovement playerMovement;

    float inputBlockUntil = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        dialogueBox.SetActive(false);
        continueButton.SetActive(false);

        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    public void StartDialogue(DialogueData data)
    {
        if (data == null)
        {
            Debug.LogError("DialogueManager: DialogueData es NULL.");
            return;
        }

        currentDialogue = data;
        index = 0;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        dialogueBox.SetActive(true);
        continueButton.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = false;

        inputBlockUntil = Time.time + 0.1f;

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        continueButton.SetActive(false);

        dialogueText.text = "";

        string line = currentDialogue.lines[index].text;
        AudioClip lineSound = currentDialogue.lines[index].sound;

        if (audioSource != null)
            audioSource.Stop();

        if (audioSource != null && lineSound != null)
        {
            audioSource.clip = lineSound;
            audioSource.Play();
        }

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        continueButton.SetActive(true);
    }

    void PlayClick()
    {
        if (clickSource == null) return;
        if (clickSound == null) return;

        clickSource.PlayOneShot(clickSound);
    }

    public void NextLine()
    {
        PlayClick(); // ← CLICK AQUÍ

        if (audioSource != null)
            audioSource.Stop();

        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentDialogue.lines[index].text;
            isTyping = false;
            continueButton.SetActive(true);
            return;
        }

        index++;

        if (index < currentDialogue.lines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        dialogueBox.SetActive(false);
        continueButton.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;
    }

    void Update()
    {
        if (!dialogueBox.activeSelf) return;

        if (Time.time < inputBlockUntil)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
            NextLine();
    }

    public bool IsDialogueOpen()
    {
        return dialogueBox.activeSelf;
    }
}
