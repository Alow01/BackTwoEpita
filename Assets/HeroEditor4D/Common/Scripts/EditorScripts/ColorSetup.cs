using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor4D.Common.Scripts.EditorScripts
{
    public class ColorSetup : MonoBehaviour
    {
        public Slider Hue;
        public Slider Saturation;
        public Slider Brightness;
        public Color Color;

        public Action<float, float, float> OnColorChanged;

        public void OnSliderChanged()
        {
            OnColorChanged?.Invoke(Hue.value, Saturation.value, Brightness.value);
        }
    }
}