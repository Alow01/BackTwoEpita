using System;
using Assets.HeroEditor4D.InventorySystem.Scripts.Enums;

namespace Assets.HeroEditor4D.InventorySystem.Scripts.Data
{
    [Serializable]
    public class Modifier
    {
        public ItemModifier Id;
        public int Level;

        public Modifier()
        {
        }

        public Modifier(ItemModifier id, int level)
        {
            Id = id;
            Level = level;
        }
    }
}