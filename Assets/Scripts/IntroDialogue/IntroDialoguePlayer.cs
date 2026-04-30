using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroDialoguePlayer : MonoBehaviour
{
    public IntroDialogueData dialogue;
    public TextMeshProUGUI textUI;
    public float typingSpeed = 0.03f;

    public Button nextButton;
    public Button startButton;

    public AudioSource audioSource;   // Opcional
    public AudioClip clickSound;      // Opcional

    private int index = 0;
    private bool isTyping = false;

    void Start()
    {
        nextButton.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);

        nextButton.onClick.AddListener(() => { PlayClick(); NextDialogue(); });
        startButton.onClick.AddListener(() => { PlayClick(); StartGame(); });

        ShowLine();
    }

    void PlayClick()
    {
        // Protección total: si falta algo, no hace nada
        if (audioSource == null) return;
        if (clickSound == null) return;

        audioSource.PlayOneShot(clickSound);
    }

    void ShowLine()
    {
        // Si estamos en la última línea, cambiamos los botones YA
        if (index == dialogue.lines.Length - 1)
        {
            nextButton.gameObject.SetActive(false);
            startButton.gameObject.SetActive(true);
        }
        else
        {
            nextButton.gameObject.SetActive(true);
            startButton.gameObject.SetActive(false);
        }

        StopAllCoroutines();
        StartCoroutine(TypeLine(dialogue.lines[index]));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        textUI.text = "";

        foreach (char c in line)
        {
            textUI.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    public void NextDialogue()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            textUI.text = dialogue.lines[index];
            isTyping = false;
            return;
        }

        index++;

        if (index >= dialogue.lines.Length)
        {
            index = dialogue.lines.Length - 1;
            return;
        }

        ShowLine();
    }

    void StartGame()
    {
        MenuScript.sceneToLoad = "Level1";
        SceneManager.LoadScene("LoadingScreen");
    }
}
