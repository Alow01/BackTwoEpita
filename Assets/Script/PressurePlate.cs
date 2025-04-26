using UnityEngine;
using System;

public class PressurePlate : MonoBehaviour
{
    private EgSymbManager parentScript;
    private string name = "";

    private void Start()
    {
        parentScript = GetComponentInParent<EgSymbManager>();
        name = this.gameObject.name;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        parentScript.OnPlateTriggered(other,name);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        parentScript.OnPlateUntriggered(other, name);
    }
}
