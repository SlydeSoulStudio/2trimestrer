using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    public GameObject heartPrefab;
    public GameObject breakEffectPrefab;
    public AudioClip breakSound;

    public void BreakBox()
    {
        if (breakEffectPrefab != null)
            Instantiate(breakEffectPrefab, transform.position, Quaternion.identity);

        if (breakSound != null)
            AudioSource.PlayClipAtPoint(breakSound, transform.position);

        if (heartPrefab != null)
            Instantiate(heartPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);
    }
}
