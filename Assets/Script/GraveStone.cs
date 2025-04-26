using Mirror.Examples.BilliardsPredicted;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Cainos.PixelArtTopDown_Basic
{
    //when object exit the trigger, put it to the assigned layer and sorting layers
    //used in the stair objects for player to travel between layers
    public class GraveStone : MonoBehaviour
    {
        public Image textToPrint;
        public InputAction touchKeyEAction; // Replaced InputSystem with InputAction to fix CS0723

        public void OnTriggerEnter2D(Collider2D collision) // Fixed spelling error in method name
        {
            if (touchKeyEAction != null && touchKeyEAction.WasPerformedThisFrame() && collision.gameObject.CompareTag("Player"))
            {
                if (textToPrint != null)
                {
                    textToPrint.enabled = true;
                }
            }
        }

        public void Start()
        {
            if (textToPrint != null)
            {
                textToPrint.enabled = false; // Masquer l'image au début
            }
        }
    }
}
