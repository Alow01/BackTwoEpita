using Mirror;
using UnityEngine;

public class WellManager : MonoBehaviour
{
    private AudioSource audioSource;
    public EgypteEnigma2 manager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        NetworkIdentity identity = other.GetComponent<NetworkIdentity>();
        if (identity != null && identity.isLocalPlayer)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null && !audioSource.isPlaying)
                audioSource.Play();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        NetworkIdentity identity = other.GetComponent<NetworkIdentity>();
        if (identity != null && identity.isLocalPlayer)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
                audioSource.Stop();
        }
    }
}
