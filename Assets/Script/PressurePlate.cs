using UnityEngine;
using System;

public class PressurePlate : MonoBehaviour
{
    private EgSymbManager parentScript;

    

    private void Start()
    {
        parentScript = GetComponentInParent<EgSymbManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag != "Player") parentScript.OnPlateTriggered(other, this.gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player") parentScript.OnPlateUntriggered(other, this.gameObject.name);
    }
}
