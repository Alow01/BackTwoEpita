using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//when something get into the alta, make the runes glow
namespace Cainos.PixelArtTopDown_Basic
{

    public class PropsAltar : MonoBehaviour
    {
        public List<SpriteRenderer> runes;
        public float lerpSpeed;

        private Color curColor;
        private Color targetColor;
        private bool trigger;


        private void Awake()
        {
            targetColor = runes[0].color;
            trigger = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            targetColor.a = 1.0f;
            trigger = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            targetColor.a = 0.0f;
            trigger = false;
        }

        private void Update()
        {
            if (!trigger)
            {
                // Calculate the alpha value using a sinusoidal function
                float alpha = 1f + 0.5f * Mathf.Sin(Time.time * (lerpSpeed + 1));   // Oscillates between 0 and 1

                // Update the current color's alpha value
                curColor = runes[0].color; // Keep the base color of the runes
                curColor.a = alpha;

                // Apply the updated color to all runes
                foreach (var r in runes)
                {
                    r.color = curColor;
                }
            }
            else
            {
                curColor = Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);

                foreach (var r in runes)
                {
                    r.color = curColor;
                }
            }
        }

    }
}
