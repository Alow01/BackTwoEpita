using UnityEngine;
using UnityEngine.InputSystem;

public class AllumagePillier : MonoBehaviour
{
    public GameObject Pillier;
    public InputAction touchKeyEAction;


    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D method called");
        Transform glowTransform = Pillier.transform.Find("Glow");
        if (touchKeyEAction != null && touchKeyEAction.WasPerformedThisFrame() && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger and E key was pressed");
            if (glowTransform != null)
            {
                glowTransform.gameObject.SetActive(true);
            }
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
