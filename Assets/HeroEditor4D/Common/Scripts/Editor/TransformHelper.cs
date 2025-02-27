using System;
using UnityEngine;

namespace HeroEditor4D.Common.Editor
{
    [ExecuteInEditMode]
    public class TransformNormalizer : MonoBehaviour
    {
        public bool ZeroZ;
        public bool RoundTransform;
        public int RoundTransformDigits;

        public void OnEnable()
        {
            if (ZeroZ)
            {
                foreach (var t in GetComponentsInChildren<Transform>())
                {
                    t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, 0);
                }
            }

            if (RoundTransform)
            {
                foreach (var t in GetComponentsInChildren<Transform>())
                {
                    t.localPosition = new Vector3(
                        (float) Math.Round(t.localPosition.x, RoundTransformDigits),
                        (float) Math.Round(t.localPosition.y, RoundTransformDigits),
                        (float) Math.Round(t.localPosition.z, RoundTransformDigits));
                }
            }
        }
    }
}