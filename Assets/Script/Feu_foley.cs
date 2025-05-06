using UnityEngine;

public class Feu_foley : MonoBehaviour
{
    public bool isTouch;

    private void Start()
    {
        isTouch = false;
        this.gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouch = true;
        }
    }
}
