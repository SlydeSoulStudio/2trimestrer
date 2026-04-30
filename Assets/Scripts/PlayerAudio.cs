using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource pickupAudio;

    public void PlayPickupSound(AudioClip clip)
    {
        pickupAudio.PlayOneShot(clip);
    }
}