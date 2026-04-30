using UnityEngine;
using TMPro;
using System.Collections;
using Unity.Cinemachine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    int currentHealth;

    [Header("UI")]
    public TextMeshProUGUI healthText;

    [Header("Damage Effects")]
    public Renderer playerRenderer;
    public Color hitColor = Color.red;
    public float hitFlashDuration = 0.1f;

    [Header("Camera Shake")]
    public CinemachineImpulseSource impulseSource;

    [Header("Audio")]
    public AudioClip deathSound;
    public AudioClip hitSound;
    public AudioSource audioSource;

    [Header("Hit VFX")]
    public GameObject hitEffectPrefab;

    [Header("Death VFX")]
    public GameObject deathEffectPrefab;

    Animator anim;
    GameControl gameControl;

    public bool isDead = false;
    Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        anim = GetComponent<Animator>();
        gameControl = FindFirstObjectByType<GameControl>();

        if (playerRenderer != null)
            originalColor = playerRenderer.material.color;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        if (anim != null)
        {
            anim.ResetTrigger("Hit");
            anim.ResetTrigger("Die");
        }

        if (currentHealth - amount <= 0)
        {
            currentHealth -= amount;
            UpdateHealthUI();

            isDead = true;
            StartCoroutine(DieSequence());
            return;
        }

        currentHealth -= amount;
        UpdateHealthUI();

        if (anim != null)
            anim.SetTrigger("Hit");

        if (impulseSource != null)
            impulseSource.GenerateImpulse();

        if (playerRenderer != null)
            StartCoroutine(HitFlash());

        if (hitEffectPrefab != null)
        {
            Vector3 spawnPos = playerRenderer != null
                ? playerRenderer.bounds.center
                : transform.position;

            Instantiate(hitEffectPrefab, spawnPos, Quaternion.identity);
        }

        if (audioSource != null && hitSound != null)
            audioSource.PlayOneShot(hitSound);
    }

    IEnumerator HitFlash()
    {
        playerRenderer.material.color = hitColor;
        yield return new WaitForSeconds(hitFlashDuration);
        playerRenderer.material.color = originalColor;
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = "" + currentHealth;
    }

    IEnumerator DieSequence()
    {
        if (!isDead) yield break;

        Debug.Log("Player died");

        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        var cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        anim.applyRootMotion = false;
        anim.ResetTrigger("Hit");
        anim.SetTrigger("Die");

        if (deathEffectPrefab != null)
        {
            Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
            Instantiate(deathEffectPrefab, spawnPos, Quaternion.identity);
        }


        yield return null;
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        while (!state.IsName("Die"))
        {
            yield return null;
            state = anim.GetCurrentAnimatorStateInfo(0);
        }

        yield return new WaitForSeconds(state.length);

        //reset coins 
        GameData.coins = 0;
        GameData.coinsForLife = 0;

        if (gameControl != null)
            gameControl.ShowGameOver();
        else
            Debug.LogError("PlayerHealth: gameControl es NULL");
    }

    public void ResetAfterRespawn(Vector3 respawnPosition)
    {
        isDead = false;

        transform.position = respawnPosition;

        var cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = true;

        if (anim != null)
        {
            anim.ResetTrigger("Die");
            anim.Play("Locomotion");
        }

        ResetHealth();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void RecoverHealth(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        UpdateHealthUI();
    }
}
