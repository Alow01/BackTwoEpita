using UnityEngine;
using UnityEngine.InputSystem;

public class AllumagePillier : MonoBehaviour
{
    public GameObject Pillier;
    public InputAction touchKeyEAction;
    public bool activate =false;
    public bool IsInRange = false;

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
            IsInRange = true;
            touchKeyEAction.performed -= HandleKeyPress;
            touchKeyEAction.performed += HandleKeyPress;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Unsubscribe from the event when the player exits the trigger
            IsInRange = false;
            touchKeyEAction.performed -= HandleKeyPress;
        }
    }


    private void HandleKeyPress(InputAction.CallbackContext ctx)
    {
        if (IsInRange) 
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
