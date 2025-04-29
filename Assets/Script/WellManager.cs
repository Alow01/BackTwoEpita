using UnityEngine;

public class WellManager : MonoBehaviour
{
    public AudioSource audioSource;
    private void OnTriggerEnter2D(Collider2D other)
    {
        audioSource.Play();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        audioSource.Stop();
    }
}
