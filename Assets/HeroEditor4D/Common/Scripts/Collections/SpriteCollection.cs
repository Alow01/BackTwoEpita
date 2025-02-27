using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.Data;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.Collections
{
    /// <summary>
    /// Collects sprites from specified path.
    /// </summary>
    [CreateAssetMenu(fileName = "SpriteCollection", menuName = "HeroEditor4D/SpriteCollection")]
    public class SpriteCollection : ScriptableObject
    {
        public string Id;

        [Header("Where to find sprites?")]
        public List<Object> SpriteFolders;
        public List<string> CollectionFilter;
        public List<string> CollectionFilterIgnore;

        [Header("Body Parts")]
        public List<ItemSprite> Body;
        public List<ItemSprite> Ears;
        public List<ItemSprite> Hair;
        public List<ItemSprite> Beard;
        public List<ItemSprite> Eyebrows;
        public List<ItemSprite> Eyes;
        public List<ItemSprite> Mouth;
        
        [Header("Equipment")]
        public List<ItemSprite> Armor;
        public List<ItemSprite> Cape;
        public List<ItemSprite> Backpack;
        public List<ItemSprite> MeleeWeapon1H;
        public List<ItemSprite> MeleeWeapon2H;
        public List<ItemSprite> Bow;
	    public List<ItemSprite> Crossbow;
        public List<ItemSprite> Firearm1H;
        public List<ItemSprite> Firearm2H;
        public List<ItemSprite> Shield;
        public List<ItemSprite> Throwable;
        public List<ItemSprite> Supplies;

        [Header("Accessories")]
        public List<ItemSprite> Makeup;
        public List<ItemSprite> Mask;
        public List<ItemSprite> Earrings;

        [Header("Service")]
        public bool IncludePsd;
		public bool DebugLogging;

        public List<ItemSprite> GetAllSprites()
        {
            return Body.Union(Ears).Union(Hair).Union(Beard).Union(Eyebrows).Union(Eyes).Union(Mouth)
                .Union(Armor).Union(Cape).Union(Backpack).Union(MeleeWeapon1H).Union(MeleeWeapon2H)
                .Union(Bow).Union(Crossbow).Union(Firearm1H).Union(Firearm2H).Union(Shield).Union(Throwable).Union(Supplies)
                .Union(Makeup).Union(Mask).Union(Earrings).ToList();
        }
    }
}