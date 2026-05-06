using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    int currentHealth;

    [Header("Components")]
    public Animator animator;
    public AudioSource audioSource;
    public Renderer enemyRenderer;

    [Header("Sounds")]
    public AudioClip hitSound;
    public AudioClip DamagedSound;
    public AudioClip deathSound;

    [Header("Hit Flash")]
    public Color hitColor = Color.red;
    public float hitFlashDuration = 0.1f;

    [Header("Death Settings")]
    public float destroyDelay = 2f;

    [Header("Death VFX")]
    public GameObject deathParticles;

    [Header("Drop Settings")]
    public GameObject dropPrefab;   // objeto que va a spawnear al morir
    public Vector3 dropOffset = new Vector3(0, 0.5f, 0); // lugar del spawn

    bool isDead = false;
    Color originalColor;

    Coroutine reenableAICoroutine;

    void Start()
    {
        currentHealth = maxHealth;

        if (animator == null)
            animator = GetComponent<Animator>();

        if (enemyRenderer != null)
            originalColor = enemyRenderer.material.color;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        // si muere, NO reproducir Hit, NO reactivar IA
        if (currentHealth <= 0)
        {
            StartCoroutine(DieSequence());
            return;
        }

        // --- sonido de impacto desde el jugador ---
        var playerAudio = FindObjectOfType<PlayerHealth>()?.GetComponent<AudioSource>();
        if (playerAudio != null && hitSound != null)
            playerAudio.PlayOneShot(hitSound);

        // animación de impacto (solo si hay animator)
        if (animator != null)
            animator.SetTrigger("Hit");

        // desactivar IA
        var ai = GetComponent<EnemyFollow2>();
        if (ai != null) ai.enabled = false;

        // flash rojo
        if (enemyRenderer != null)
            StartCoroutine(HitFlash());

        // reactivar IA después del Hit
        if (reenableAICoroutine != null)
            StopCoroutine(reenableAICoroutine);

        reenableAICoroutine = StartCoroutine(ReenableAI());
    }

    IEnumerator ReenableAI()
    {
        yield return new WaitForSeconds(0.4f);

        var ai = GetComponent<EnemyFollow2>();
        if (ai != null && !isDead)
            ai.enabled = true;
    }

    IEnumerator HitFlash()
    {
        enemyRenderer.material.color = hitColor;
        yield return new WaitForSeconds(hitFlashDuration);
        enemyRenderer.material.color = originalColor;
    }

    IEnumerator DieSequence()
    {
        isDead = true;

        // evita que el golpe solape la animacion de muerte
        if (animator != null)
            animator.ResetTrigger("Hit");

        // desactivar colisiones para no volver a golpearlo una vez muerto
        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = false;

        // cancelar solo la corrutina de IA
        if (reenableAICoroutine != null)
            StopCoroutine(reenableAICoroutine);

        // detener IA y movimiento
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.isStopped = true;

        var follow = GetComponent<EnemyFollow2>();
        if (follow != null) follow.enabled = false;

        // --- sonido de muerte desde el jugador ---
        var playerAudio = FindObjectOfType<PlayerHealth>()?.GetComponent<AudioSource>();
        if (playerAudio != null && deathSound != null)
            playerAudio.PlayOneShot(deathSound);

        // animación de muerte (solo si hay animator)
        if (animator != null)
        {
            animator.SetTrigger("Die");

            // esperar a que entre en Die
            yield return null;
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            while (!state.IsName("Die"))
            {
                yield return null;
                state = animator.GetCurrentAnimatorStateInfo(0);
            }

            // esperar a que termine
            yield return new WaitForSeconds(state.length);
        }

        // partículas de muerte
        if (deathParticles != null)
            Instantiate(deathParticles, transform.position, Quaternion.identity);

        // si es una caja, romperla
        var box = GetComponent<BreakableBox>();
        if (box != null)
            box.BreakBox();

        // spawn del objeto al morir
        if (dropPrefab != null)
            Instantiate(dropPrefab, transform.position + dropOffset, Quaternion.identity);

        Destroy(gameObject, destroyDelay);
    }
}
