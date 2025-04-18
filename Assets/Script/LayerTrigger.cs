﻿using Mirror.Examples.BilliardsPredicted;
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
    public class LayerTrigger : MonoBehaviour
    {
        public Image textToPrint;
        public InputAction action;

        public UnityEngine.InputSystem.PlayerInput playerInput;
        public void nTriggerEnter2D(Collider2D collision)
        {
            action = playerInput.actions["TouchKeyE"];
            if (action.WasPerformedThisFrame() && collision.gameObject.CompareTag("Player"))
            {
                if (textToPrint != null )
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
