using UnityEngine;

public class WellManager : MonoBehaviour
{
    public AudioSource audioSource;
    public EgypteEnigma2 manager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        audioSource.Play();
        //if(this.gameObject.name != "WellP1") manager.q.Enqueue(this.gameObject.name);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        audioSource.Stop();
    }
}
