using UnityEngine;
using System.Collections;

public class PlayerKnockback : MonoBehaviour
{
    public bool isKnocked = false;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ApplyKnockback(Vector3 direction, float force, float duration)
    {
        if (!isKnocked)
            StartCoroutine(KnockbackRoutine(direction, force, duration));
    }

    IEnumerator KnockbackRoutine(Vector3 direction, float force, float duration)
    {
        isKnocked = true;

        // Cancelar movimiento del jugador
        rb.linearVelocity = Vector3.zero;

        // Aplicar impulso
        rb.AddForce(direction * force, ForceMode.Impulse);

        yield return new WaitForSeconds(duration);

        isKnocked = false;
    }
}
