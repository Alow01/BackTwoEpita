using UnityEngine;
using UnityEngine.InputSystem;

public class AllumagePillier : MonoBehaviour
{
    public GameObject Pillier;
    public InputAction touchKeyEAction;
    private bool activate;

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
        if (collision.gameObject.CompareTag("Player"))
        {
            // Subscribe to the event only once
            touchKeyEAction.performed -= HandleKeyPress;
            touchKeyEAction.performed += HandleKeyPress;
        }
    }

    private void HandleKeyPress(InputAction.CallbackContext ctx)
    {
        Transform glowTransform = Pillier.transform.Find("Glow");
        if (glowTransform != null)
        {
            if (!activate)
            {
                glowTransform.gameObject.SetActive(true); // Turn on the glow
                Debug.Log("Glow object activated");
                activate = true;
            }
            else
            {
                glowTransform.gameObject.SetActive(false); // Turn off the glow
                Debug.Log("Glow object deactivated");
                activate = false;
            }
        }
    }

    public void Start()
    {
        Transform glowTransform = Pillier.transform.Find("Glow");
        if (glowTransform != null)
        {
            glowTransform.gameObject.SetActive(false);
        }
        activate = false;
    }
}
