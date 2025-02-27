using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.InventorySystem.Scripts;
using Assets.HeroEditor4D.InventorySystem.Scripts.Data;
using Assets.HeroEditor4D.InventorySystem.Scripts.Elements;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.ExampleScripts
{
    public class InventoryExample : MonoBehaviour
    {
        public ItemCollection ItemCollection;
        public ScrollInventory Inventory;
        public Character4D Character;
        public AppearanceExample AppearanceExample;

        public void Awake()
        {
            // You must to set an active collection (as there may be several different collections in Resources).
            ItemCollection.Active = ItemCollection;
        }

        public void Start()
        {
            var items = ItemCollection.Items.Select(i => new Item(i.Id)).ToList();

            InventoryItem.OnLeftClick = item =>
            {
                Character.Equip(item);
                AppearanceExample.Refresh();
            };
            Inventory.Initialize(ref items);
        }
    }
}