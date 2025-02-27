using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor4D.Common.Scripts.EditorScripts
{
    /// <summary>
    /// Used to set Slider zero value.
    /// </summary>
    public class SliderReset : MonoBehaviour
    {
        public void Reset()
        {
            GetComponentInParent<Slider>().value = 0;
        }
    }
}