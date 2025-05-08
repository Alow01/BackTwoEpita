using Mirror;
using UnityEngine;
using System;

public class EgSymbManager : MonoBehaviour
{
    public GameObject door;

    private bool hasBeenOpen;

    private bool press1;
    private bool press2;
    private bool press3;
    private bool press4;
    private bool press5;

    private void Start()
    {
        hasBeenOpen = false;
        press1 = false;
        press2 = false;
        press3 = false;
        press4 = false;
        press5 = false;
    }
    void Update()
    {
        if (!hasBeenOpen) {

            bool condition = press1 && press2 && press3 && press4 && press5;

            if (condition) {
                Debug.Log("All symbols has been press at the same time -> Success enigma -> Door P2 is now open");
                hasBeenOpen = true;
                MoveDoor();               
            }
        }

    }
    public void OnPlateTriggered(Collider2D other, string name)
    {

        switch (name)
        {
            case ("P1"): press1 = true; break;
            case ("P2"): press2 = true; break;
            case ("P3"): press3 = true; break;
            case ("P4"): press4 = true; break;
            case ("P5"): press5 = true; break;
            default: Debug.Log("Wrong Name"); break;
        }
        Debug.Log($"Pressure plate {name} has been pressed !");
        
    }
    public void OnPlateUntriggered(Collider2D other, string name)
    {
        switch (name)
        {
            case ("P1"): press1 = false; break;
            case ("P2"): press2 = false; break;
            case ("P3"): press3 = false; break;
            case ("P4"): press4 = false; break;
            case ("P5"): press5 = false; break;
            default: Debug.Log("Wrong Name"); break;
        }
        Debug.Log($"Pressure plate {name} has been released !");
    }

    private void MoveDoor()
    {
        door.GetComponent<Transform>().position = new Vector3(200, -200, 0);
    }

}
