using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.CharacterScripts
{
    /// <summary>
    /// Used to associate SpriteRenderer with SpriteCollection.
    /// </summary>
	public class SpriteMapping : MonoBehaviour
	{
		public string SpriteName;
        public List<string> SpriteNameFallback;
        
        /// <summary>
        /// Find sprite by SpriteName, then by SpriteNameIfNotFound. Return null if nothing found.
        /// </summary>
        public Sprite FindSprite(List<Sprite> sprites)
        {
            if (sprites == null || sprites.Count == 0) return null;

            return sprites.SingleOrDefault(i => i != null && i.name == SpriteName) ?? sprites.SingleOrDefault(i => i != null && SpriteNameFallback.Contains(i.name));
        }
    }
}