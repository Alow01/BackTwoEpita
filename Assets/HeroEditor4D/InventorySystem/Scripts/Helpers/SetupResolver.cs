using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.InventorySystem.Scripts.Elements;
using UnityEngine;

namespace Assets.HeroEditor4D.InventorySystem.Scripts.Helpers
{
    public class SetupResolver : MonoBehaviour
    {
        public Character4D Character;
        public Equipment Equipment;
        public ItemWorkspace ItemWorkspace;
        public List<Character4D> Characters;
        public List<ItemCollection> ItemCollections;
        
        /// <summary>
        /// The main point of this method is to place a correct existing prefab on a scene.
        /// </summary>
        public void Awake()
        {
            if (Character != null)
            {
                Destroy(Character.gameObject);

                Character = Instantiate(Characters.First(i => i != null));
                Character.transform.position = new Vector3(0, 2.5f);
                Character.SetDirection(Vector2.down);

                if (Equipment) Equipment.Preview = Character.Front;
            }
            
            if (ItemWorkspace) ItemWorkspace.ItemCollection = ItemCollections.First(i => i != null);
        }
    }
}