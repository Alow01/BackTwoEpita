using System.Collections.Generic;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.ExampleScripts
{
    /// <summary>
    /// General behaviour for projectiles: bullets, rockets and other.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        public List<Renderer> Renderers;
        public GameObject Trail;
        public GameObject Impact;
        public Rigidbody Rigidbody;

        public void Start()
        {
            Destroy(gameObject, 5);
        }

        public void Update()
        {
            if (Rigidbody != null && Rigidbody.useGravity)
            {
                transform.right = Rigidbody.linearVelocity.normalized;
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            Bang(other.gameObject);
        }

        public void OnCollisionEnter2D(Collision2D other)
        {
            Bang(other.gameObject);
        }

        private void Bang(GameObject other)
        {
            ReplaceImpactSound(other);
            Impact.SetActive(true);
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<Collider>());
            Destroy(gameObject, 1);

            foreach (var ps in Trail.GetComponentsInChildren<ParticleSystem>())
            {
                ps.Stop();
            }

            foreach (var tr in Trail.GetComponentsInChildren<TrailRenderer>())
            {
                tr.enabled = false;
            }
        }

        private void ReplaceImpactSound(GameObject other)
        {
            var sound = other.GetComponent<AudioSource>();

            if (sound != null && sound.clip != null)
            {
                Impact.GetComponent<AudioSource>().clip = sound.clip;
            }
        }
    }
}