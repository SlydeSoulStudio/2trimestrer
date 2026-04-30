using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public int damage = 20;
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Daño
            PlayerHealth hp = other.GetComponentInParent<PlayerHealth>();
            if (hp != null)
                hp.TakeDamage(damage);

            // 2. Knockback real
            PlayerKnockback kb = other.GetComponent<PlayerKnockback>();
            if (kb != null)
            {
                Vector3 dir = (other.transform.position - transform.position).normalized;
                kb.ApplyKnockback(dir, knockbackForce, knockbackDuration);
            }
        }
    }
}
