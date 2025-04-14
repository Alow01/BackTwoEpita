using UnityEngine;
using UnityEngine.InputSystem;

public class AllumagePillier : MonoBehaviour
{
    public GameObject Pillier;
    public InputAction touchKeyEAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnTriggerEnter2D(Collider2D collision) // Fixed spelling error in method name
    {
        Transform glowTransform = Pillier.transform.Find("Glow");
        if (touchKeyEAction != null && touchKeyEAction.WasPerformedThisFrame() && collision.gameObject.CompareTag("Player"))
        {
            if (glowTransform != null)
            {
                glowTransform.gameObject.SetActive(true); // Activate the glow object
            }
        }
    }

    public void Start()
    {
        Transform glowTransform = Pillier.transform.Find("Glow");
        if (glowTransform != null)
        {
            glowTransform.gameObject.SetActive(false); // Desactivate the glow object
        }
    }
}
