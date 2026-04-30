using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackCooldown = 0.6f;
    public int attackDamage = 50;
    public float attackRange = 2f;
    public float forwardOffset = 0f;
    public LayerMask enemyLayers;

    [Header("Sound Settings")]
    public AudioClip attackClip;
    private AudioSource audioSource;

    [Header("VFX")]
    public GameObject hitEffectPrefab;

    [Header("References")]
    public Animator animator;

    [Header("Attack VFX")]
    public GameObject attackEffectPrefab;
    public Transform attackEffectPoint; // donde aparece el efecto

    private float nextAttackTime = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // FIX BLOQUEO TOTAL SI EL JUGADOR ESTÁ MUERTO
        PlayerHealth health = GetComponent<PlayerHealth>();
        if (health != null && health.isDead)
            return;

        //input for attack
        bool attackInput = Keyboard.current.fKey.wasPressedThisFrame;

        if (attackInput && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Attack()
    {
        // evitar ataques desde Animation Events si está muerto
        PlayerHealth health = GetComponent<PlayerHealth>();
        if (health != null && health.isDead)
            return;

        animator.SetTrigger("Attack");

        // efecto visual del ataque (cola girando)
        if (attackEffectPrefab != null)
        {
            Vector3 spawnPos = attackEffectPoint != null
                ? attackEffectPoint.position
                : transform.position + transform.forward * 0.5f;

            Instantiate(attackEffectPrefab, spawnPos, transform.rotation);
        }

        Vector3 attackPos = transform.position + transform.forward * forwardOffset;

        Collider[] hitEnemies = Physics.OverlapSphere(attackPos, attackRange, enemyLayers);
        Debug.Log("HitEnemies count: " + hitEnemies.Length);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);

                // --- SONIDO DE DAÑO DESDE EL JUGADOR ---
                if (enemyHealth.DamagedSound != null)
                    audioSource.PlayOneShot(enemyHealth.DamagedSound);
            }

            // efecto de golpe
            if (hitEffectPrefab != null)
            {
                Vector3 spawnPos = enemy.ClosestPoint(transform.position);
                Instantiate(hitEffectPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    // play sound on animation attack fix
    public void PlayAttackSound()
    {
        //  fix bloqueaar sonido si está muerto
        PlayerHealth health = GetComponent<PlayerHealth>();
        if (health != null && health.isDead)
            return;

        if (audioSource != null && attackClip != null)
            audioSource.PlayOneShot(attackClip);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackPos = transform.position + transform.forward * forwardOffset;
        Gizmos.DrawWireSphere(attackPos, attackRange);
    }
}