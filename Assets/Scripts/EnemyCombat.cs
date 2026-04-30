using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private float lastAttackTime;
    public float attackCooldown = 2f;

    public Animator animator;          // Asignar en Inspector o se rellena en Awake
    public Transform attackPoint;      // Asignado en Inspector
    public int attackDamage = 20;
    public float attackRange = 1f;
    public LayerMask playerLayer;

    void Awake()
    {
        // Por si se te olvida asignar el Animator en el Inspector
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        if (animator != null)
            animator.SetTrigger("Attack");

        Debug.Log("Enemy dealing damage");

        if (attackPoint == null)
        {
            Debug.LogError("EnemyCombat: attackPoint no asignado");
            return;
        }

        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

        foreach (Collider col in hitPlayers)
        {
            // Blindado: aunque el collider no sea el padre, sube y busca PlayerHealth
            PlayerHealth hp = col.GetComponentInParent<PlayerHealth>();

            if (hp != null)
            {
                hp.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogWarning("EnemyCombat: collider detectado sin PlayerHealth -> " + col.name);
            }
        }
    }

    public void TryAttack()
    {
        if (Time.time >= lastAttackTime)
        {
            Attack();
            lastAttackTime = Time.time + attackCooldown;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}