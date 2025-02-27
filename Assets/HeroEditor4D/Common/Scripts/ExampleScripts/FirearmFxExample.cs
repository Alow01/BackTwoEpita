using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Collections;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.ExampleScripts
{
    public class FirearmFxExample : MonoBehaviour
    {
        public Character4D Character4D;
        public AudioSource AudioSource;
        public FirearmCollection FirearmCollection;

        private readonly List<ParticleSystem> _instances = new List<ParticleSystem>();
        private string _muzzleId;

        public void CreateFireMuzzle(string firearmName, string fallback = null)
        {
            var firearmParams = FirearmCollection.FirearmParams.SingleOrDefault(i => i.Name == firearmName)
                ?? FirearmCollection.FirearmParams.SingleOrDefault(i => i.Name == fallback)
                ?? FirearmCollection.FirearmParams.SingleOrDefault(i => i.Name == "Basic");

            if (firearmParams == null) throw new Exception($"Firearm params not found for {firearmName}. Please check FirearmCollection.");

            if (_muzzleId != firearmParams.Name)
            {
                Initialize(firearmParams);
            }

            foreach (var muzzle in _instances.Where(i => i.gameObject.activeInHierarchy))
            {
                muzzle.Play(true);
            }

            AudioSource.PlayOneShot(firearmParams.ShotSound);
        }

        private void Initialize(FirearmParams firearmParams)
        {
            if (_instances.Count > 0)
            {
                _instances.ForEach(i => Destroy(i.gameObject));
                _instances.Clear();
            }

            for (var i = 0; i < 4; i++)
            {
                var anchor = Character4D.Parts[i].AnchorFireMuzzle;
                var muzzle = Instantiate(firearmParams.FireMuzzlePrefab, anchor);

                _instances.Add(muzzle);
            }

            _muzzleId = firearmParams.Name;
        }
    }
}