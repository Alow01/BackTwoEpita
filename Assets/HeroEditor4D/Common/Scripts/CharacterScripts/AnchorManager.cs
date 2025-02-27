using System.Collections.Generic;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.CharacterScripts
{
    public class AnchorManager : MonoBehaviour
    {
        public Transform Body;
        public Transform PrimaryWeapon;
        public Transform Bow;
        public Transform Shield;
        public Transform Status;

        public List<Transform> Custom;
    }
}