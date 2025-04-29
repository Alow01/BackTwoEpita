using UnityEngine;
using UnityEngine.InputSystem;

public class AllumagePillier : MonoBehaviour
{
    public GameObject Pillier;
    public InputAction touchKeyEAction;

    private void OnEnable()
    {
        touchKeyEAction.Enable();
    }

    private void OnDisable()
    {
        touchKeyEAction.Disable();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Transform glowTransform = Pillier.transform.Find("Glow");
        if (collision.gameObject.CompareTag("Player"))
        {
            // Detect key press
            touchKeyEAction.performed += ctx =>
            {
                if (glowTransform != null)
                {
                    glowTransform.gameObject.SetActive(true); // Allume le glow
                }
            };
            touchKeyEAction.performed += ctx =>
            {
                if (glowTransform != null && glowTransform.gameObject.activeSelf)
                {
                    glowTransform.gameObject.SetActive(true); // Allume le glow
                }
            };
        }
    }

    public void Start()
    {
        Transform glowTransform = Pillier.transform.Find("Glow");
        if (glowTransform != null)
        {
            Debug.Log("Glow object found");
            glowTransform.gameObject.SetActive(false);
        }
    }
}
