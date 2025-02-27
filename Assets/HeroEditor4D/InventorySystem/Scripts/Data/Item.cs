using System;
using Assets.HeroEditor4D.Common.Scripts.Data;
using Assets.HeroEditor4D.InventorySystem.Scripts.Enums;

namespace Assets.HeroEditor4D.InventorySystem.Scripts.Data
{
    /// <summary>
    /// Represents item object for storing with game profile (please note, that item params are stored separately in params database).
    /// </summary>
    [Serializable]
    public class Item
    {
        public string Id; // Id is not unique. Use Hash to compare items!
        public Modifier Modifier;

        #if TAP_HEROES

        public ProtectedInt Count;

        #else

        public int Count;

        #endif

        public Item()
        {
        }

        public Item(string id, int count = 1)
        {
            Id = id;
            Count = count;
        }

        public Item(string id, Modifier modifier, int count = 1)
        {
            Id = id;
            Count = count;
            Modifier = modifier;
        }

        public Item Clone()
        {
            return (Item) MemberwiseClone();
        }

        public ItemParams Params => ItemCollection.Active.GetItemParams(this);
        public ItemSprite Sprite => ItemCollection.Active.GetItemSprite(this);
        public ItemIcon Icon => ItemCollection.Active.GetItemIcon(this);

        public int Hash => $"{Id}.{Modifier?.Id}.{Modifier?.Level}".GetHashCode();
        public bool IsModified => Modifier != null && Modifier.Id != ItemModifier.None;
        public bool IsEquipment => Params.Type == ItemType.Helmet || Params.Type == ItemType.Armor || Params.Type == ItemType.Vest || Params.Type == ItemType.Bracers || Params.Type == ItemType.Leggings || Params.Type == ItemType.Weapon || Params.Type == ItemType.Shield;
        public bool IsArmor => Params.Type == ItemType.Helmet || Params.Type == ItemType.Armor || Params.Type == ItemType.Vest || Params.Type == ItemType.Bracers || Params.Type == ItemType.Leggings;
        public bool IsWeapon => Params.Type == ItemType.Weapon;
        public bool IsShield => Params.Type == ItemType.Shield;
        public bool IsDagger => Params.Class == ItemClass.Dagger;
        public bool IsSword => Params.Class == ItemClass.Sword;
        public bool IsAxe => Params.Class == ItemClass.Axe;
        public bool IsPickaxe => Params.Class == ItemClass.Pickaxe;
        public bool IsWand => Params.Class == ItemClass.Wand;
        public bool IsBlunt => Params.Class == ItemClass.Blunt;
        public bool IsLance => Params.Class == ItemClass.Lance;
        public bool IsMelee => Params.Type == ItemType.Weapon && Params.Class != ItemClass.Bow && Params.Class != ItemClass.Firearm;
        public bool IsBow => Params.Class == ItemClass.Bow;
        public bool IsFirearm => Params.Class == ItemClass.Firearm;
        public bool IsOneHanded => !IsTwoHanded;
        public bool IsTwoHanded => Params.Class == ItemClass.Bow || Params.Tags.Contains(ItemTag.TwoHanded);
    }
}